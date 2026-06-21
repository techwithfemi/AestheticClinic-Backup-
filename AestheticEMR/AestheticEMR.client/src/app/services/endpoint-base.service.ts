// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Injectable, inject } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { Observable, Subject, from, throwError } from 'rxjs';
import { mergeMap, switchMap, catchError } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { User } from '../models/user.model';
import { AlertService, MessageSeverity } from './alert.service';

interface ServerError {
  status: number;
  error: {
    error: string;
    error_description: string;
  };
}


@Injectable()
export class EndpointBase {
  private authService = inject(AuthService);
  private alertService = inject(AlertService);

  private taskPauser: Subject<boolean> | null = null;
  private isRefreshingLogin = false;
  private hasShownSessionExpiredMessage = false;

  protected get requestHeaders(): { headers: HttpHeaders | Record<string, string | string[]> } {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.accessToken}`,
      'Content-Type': 'application/json',
      Accept: 'application/json, text/plain, */*'
    });

    return { headers };
  }

  protected get anonymousHeaders(): { headers: HttpHeaders | Record<string, string | string[]> } {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'application/json, text/plain, */*'
    });

    return { headers };
  }

  protected get uploadHeaders(): { headers: HttpHeaders | Record<string, string | string[]> } {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.accessToken}`,
      Accept: 'application/json, text/plain, */*'
    });

    return { headers };
  }

  public refreshLogin(): Observable<User> {
    return this.authService.refreshLogin().pipe(
      catchError((error: ServerError) => {
        return this.handleError(error, () => this.refreshLogin());
      }));
  }

  protected handleError<T>(error: ServerError, continuation: () => Observable<T>) {
    if (error.status === 401) {
      if (this.isRefreshingLogin) {
        return this.pauseTask(continuation);
      }

      this.isRefreshingLogin = true;

      return from(this.authService.refreshLogin()).pipe(
        mergeMap(() => {
          this.isRefreshingLogin = false;
          this.hasShownSessionExpiredMessage = false;
          this.resumeTasks(true);

          return continuation();
        }),
        catchError(refreshLoginError => {
          this.isRefreshingLogin = false;
          this.resumeTasks(false);
          this.notifySessionExpired();
          this.authService.reLogin();

          if (refreshLoginError.status === 401 || (refreshLoginError.error && refreshLoginError.error.error === 'invalid_grant')) {
            return throwError(() => new Error('session expired'));
          } else {
            return throwError(() => refreshLoginError || new Error('server error'));
          }
        }));
    }

    if (error.error && error.error.error === 'invalid_grant') {
      this.notifySessionExpired(error.error.error_description);
      this.authService.reLogin();

      return throwError(() => (error.error && error.error.error_description) ? `session expired (${error.error.error_description})` : 'session expired');
    } else {
      return throwError(() => error);
    }
  }

  private notifySessionExpired(reason?: string): void {
    if (this.hasShownSessionExpiredMessage) {
      return;
    }

    this.hasShownSessionExpiredMessage = true;
    const detail = reason ? `Your session expired (${reason}). Please login again.` : 'Your session expired. Please login again.';
    this.alertService.showStickyMessage('Session Expired', detail, MessageSeverity.warn);
  }

  private pauseTask<T>(continuation: () => Observable<T>) {
    if (!this.taskPauser) {
      this.taskPauser = new Subject();
    }

    return this.taskPauser.pipe(switchMap(continueOp => {
      return continueOp ? continuation() : throwError(() => new Error('session expired'));
    }));
  }

  private resumeTasks(continueOp: boolean) {
    setTimeout(() => {
      if (this.taskPauser) {
        this.taskPauser.next(continueOp);
        this.taskPauser.complete();
        this.taskPauser = null;
      }
    });
  }
}
