import { Component, HostListener, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { AppConfigService } from '../../services/app-config.service';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

interface NavigationItem {
  route?: string;
  icon?: string;
  subItems?: Array<{ path?: string; fragment?: string; label: string; icon?: string }>;
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatExpansionModule, MatButtonModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  appConfig = inject(AppConfigService);
  private readonly mobileBreakpoint = 992;

  menuEntries: Array<{ title: string; item: NavigationItem }> = [];

  get userRoles(): string[] {
    return this.authService.currentUser?.roles || [];
  }

  isSidebarCollapsed = false;
  isMobileViewport = false;
  isSidenavOpened = true;

  get fullName(): string {
    return this.authService.currentUser?.fullName || 'User';
  }

  onLogoError(img: HTMLImageElement): void {
    img.src = this.appConfig.altClientLogo;
    img.onerror = null; // prevent infinite loop if fallback also fails
  }

  private get normalizedRoles(): string[] {
    return this.userRoles.map(role => role.trim().toLowerCase()).filter(Boolean);
  }

  private get isManagementUser(): boolean {
    return this.normalizedRoles.includes('management');
  }

  private canAccessDynamicSection(sectionName: string): boolean {
    const normalizedSection = sectionName.trim().toLowerCase();

    if (normalizedSection === 'aesthetics') {
      return this.normalizedRoles.includes('aesthetics') || this.normalizedRoles.includes('laser');
    }

    return this.normalizedRoles.includes(normalizedSection);
  }

  private filterReportSubItems(subItems: Array<{ path?: string; fragment?: string; label: string; icon?: string }>): Array<{ path?: string; fragment?: string; label: string; icon?: string }> {
    if (this.isManagementUser) {
      return subItems;
    }

    const allowedPrefixes = new Set(this.normalizedRoles);

    return subItems.filter(sub => {
      const reportPrefix = (sub.path || '').split('-')[0].toLowerCase();
      return reportPrefix.length > 0 && allowedPrefixes.has(reportPrefix);
    });
  }

  @HostListener('window:resize')
  onWindowResize(): void {
    this.updateViewportState();
  }

  private updateViewportState(): void {
    const width = typeof window !== 'undefined' ? window.innerWidth : this.mobileBreakpoint;
    this.isMobileViewport = width < this.mobileBreakpoint;

    if (this.isMobileViewport) {
      this.isSidenavOpened = false;
      this.isSidebarCollapsed = false;
    }
  }

  ngOnInit(): void {
    this.updateViewportState();

    this.http.get<{ Static_Top?: Record<string, NavigationItem>; Dynamic_Roles?: Record<string, NavigationItem>; Reports?: Record<string, NavigationItem>; Settings?: Record<string, NavigationItem> }>('assets/navigation.json')
      .subscribe(json => {
        // Keep static top entries (e.g. Dashboard) even when they are root links with no subItems.
        const top = Object.entries(json.Static_Top || {})
          .map(([title, item]) => ({ title, item }));
        const dynamic = Object.entries(json.Dynamic_Roles || {})
          .filter(([roleName]) => this.canAccessDynamicSection(roleName))
          .filter(([, item]) => (item.subItems?.length || 0) > 0)
          .map(([title, item]) => ({ title, item }));
        const reports = Object.entries(json.Reports || {})
          .map(([title, item]) => ({
            title,
            item: {
              ...item,
              subItems: this.filterReportSubItems(item.subItems || [])
            }
          }))
          .filter(entry => (entry.item.subItems?.length || 0) > 0);
        const bottom = Object.entries(json.Settings || {})
          .filter(([, item]) => (item.subItems?.length || 0) > 0)
          .map(([title, item]) => ({ title, item }));

        this.menuEntries = [...top, ...dynamic, ...reports, ...bottom];
      });
  }

  toggleSidebar(): void {
    if (this.isMobileViewport) {
      this.isSidenavOpened = !this.isSidenavOpened;
      return;
    }

    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  closeSidenavOnMobile(): void {
    if (this.isMobileViewport) {
      this.isSidenavOpened = false;
    }
  }

  logout() {
    this.authService.logout();
    this.authService.redirectLogoutUser();
  }
}
