import { Component, HostListener, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../services/auth.service';
import { AppConfigService } from '../../services/app-config.service';
import { AccountService } from '../../services/account.service';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule } from '@angular/common';
import { User } from '../../models/user.model';

interface SubNavItem {
  path?: string;
  fragment?: string;
  label: string;
  icon?: string;
  serialNo?: number; // numeric order; 0 or undefined means not used
  visible?: boolean;  // visibility flag for sidebar
}

interface NavigationItem {
  route?: string;
  icon?: string;
  subItems?: SubNavItem[];
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatExpansionModule, MatButtonModule,
    MatMenuModule, TranslateModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private accountService = inject(AccountService);
  private http = inject(HttpClient);
  appConfig = inject(AppConfigService);
  private readonly mobileBreakpoint = 992;

  menuEntries: { title: string; item: NavigationItem }[] = [];

  get userRoles(): string[] {
    return this.authService.currentUser?.roles || [];
  }

  isSidebarCollapsed = false;
  isMobileViewport = false;
  isSidenavOpened = true;
  profileUser: User | null = null;

  get fullName(): string {
    return this.authService.currentUser?.fullName || 'User';
  }

  get toolbarDisplayName(): string {
    return this.profileUser?.fullName || this.profileUser?.userName || this.authService.currentUser?.fullName ||
      this.authService.currentUser?.userName || 'User';
  }

  get toolbarUserEmail(): string {
    return this.profileUser?.email || this.authService.currentUser?.email || '';
  }

  get toolbarUserPhoto(): string {
    return this.profileUser?.userPhotoBase64 || this.authService.currentUser?.userPhotoBase64 || '';
  }

  get userInitials(): string {
    const name = this.toolbarDisplayName.trim();
    if (!name) {
      return 'U';
    }

    const parts = name.split(/\s+/).filter(Boolean);
    const initials = parts.slice(0, 2).map(x => x[0]).join('').toUpperCase();
    return initials || 'U';
  }

  onLogoError(img: HTMLImageElement): void {
    img.src = this.appConfig.altClientLogo;
    img.onerror = null; // prevent infinite loop if fallback also fails
  }

  private get normalizedRoles(): string[] {
    return this.userRoles.map(role => role.trim().toLowerCase()).filter(Boolean);
  }

  private get isManagementUser(): boolean {
    return this.normalizedRoles.includes('management') || this.normalizedRoles.includes('admin');
  }

  private hasRoleAccess(roleName: string): boolean {
    const normalizedRole = roleName.trim().toLowerCase();

    if (this.isManagementUser) {
      return true;
    }

    if (normalizedRole === 'aesthetics') {
      return this.normalizedRoles.includes('aesthetics') || this.normalizedRoles.includes('laser');
    }

    const aliases = new Set<string>([normalizedRole]);
    if (normalizedRole.endsWith('s')) {
      aliases.add(normalizedRole.slice(0, -1));
    } else {
      aliases.add(`${normalizedRole}s`);
    }

    return this.normalizedRoles.some(role => aliases.has(role));
  }

  private canAccessDynamicSection(sectionName: string): boolean {
    return this.hasRoleAccess(sectionName);
  }

  /**
   * Process subItems: remove those with visible===false, and determine ordering.
   * Sort by numeric serialNo (default 0) ascending, then by label for ties.
   */
  private processSubItems(subItems: SubNavItem[] = []): SubNavItem[] {
    const visibleItems = (subItems || []).filter(s => s.visible !== false);
    if (!visibleItems || visibleItems.length === 0) {
      return [];
    }

    return visibleItems.slice().sort((a, b) => {
      const sa = typeof a.serialNo === 'number' ? a.serialNo : 0;
      const sb = typeof b.serialNo === 'number' ? b.serialNo : 0;
      if (sa !== sb) return sa - sb;
      return (a.label || '').localeCompare(b.label || '');
    });
  }

  private filterReportSubItems(subItems: SubNavItem[]): SubNavItem[] {
    // First, apply visibility filtering and role-based filtering
    if (this.isManagementUser) {
      return this.processSubItems(subItems);
    }

    const allowedRoles = new Set(this.normalizedRoles);

    const roleFiltered = (subItems || []).filter(sub => {
      if (sub.visible === false) return false;
      const reportPrefix = (sub.path || '').split('-')[0].toLowerCase();
      if (!reportPrefix) {
        return false;
      }

      if (reportPrefix === 'aesthetics') {
        return allowedRoles.has('aesthetics') || allowedRoles.has('laser');
      }

      return allowedRoles.has(reportPrefix) || allowedRoles.has(reportPrefix.endsWith('s') ? reportPrefix.slice(0, -1) : `${reportPrefix}s`);
    });

    return this.processSubItems(roleFiltered);
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
    this.loadCurrentUserProfile();
    this.accountService.getCurrentUserProfileUpdatedEvent()
      .subscribe(user => {
        this.profileUser = user;
      });

    this.http.get<{ Static_Top?: Record<string, NavigationItem>; Dynamic_Roles?: Record<string, NavigationItem>; Reports?: Record<string, NavigationItem>; Settings?: Record<string, NavigationItem> }>('assets/navigation.json')
      .subscribe(json => {
        // Canonical navigation lives at public/assets/navigation.json (served at /assets/navigation.json).
        const top = Object.entries(json.Static_Top || {})
          .map(([title, item]) => ({ title, item: { ...item, subItems: this.processSubItems(item.subItems || []) } }));

        const dynamic = Object.entries(json.Dynamic_Roles || {})
          .map(([title, item]) => ({ title, item: { ...item, subItems: this.processSubItems(item.subItems || []) } }))
          .filter(entry => this.canAccessDynamicSection(entry.title) && (entry.item.subItems?.length || 0) > 0);

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
          .map(([title, item]) => ({ title, item: { ...item, subItems: this.processSubItems(item.subItems || []) } }))
          .filter(entry => (entry.item.subItems?.length || 0) > 0);

        this.menuEntries = [...top, ...dynamic, ...reports, ...bottom];
      });
  }

  private loadCurrentUserProfile(): void {
    this.accountService.getUser()
      .subscribe({
        next: user => {
          this.profileUser = user;
        },
        error: () => {
          this.profileUser = this.authService.currentUser;
        }
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
