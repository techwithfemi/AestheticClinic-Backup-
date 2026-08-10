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
  group?: string;     // optional group label (used by Reports sidebar to cluster by module/role)
}

interface NavigationItem {
  route?: string;
  icon?: string;
  serialNo?: number;
  visible?: boolean;
  subItems?: SubNavItem[];
}

interface SubNavGroup {
  name: string;
  icon: string;
  items: SubNavItem[];
}

interface MenuEntry {
  title: string;
  item: NavigationItem;
  subGroups?: SubNavGroup[];
}

interface MenuSection {
  key: 'top' | 'dynamic' | 'bottom';
  entries: MenuEntry[];
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

  private static readonly sidebarIconPalette: Record<string, string> = {
    dashboard: '#2563eb',
    home: '#2563eb',
    frontdesk: '#0ea5e9',
    dental: '#14b8a6',
    aesthetics: '#ec4899',
    laser: '#f59e0b',
    spa: '#a855f7',
    billing: '#10b981',
    accounting: '#4f46e5',
    reports: '#0284c7',
    employees: '#06b6d4',
    'staff-roster': '#7c3aed',
    clockin: '#f97316',
    settings: '#6366f1',
    admin: '#ef4444'
  };

  menuEntries: MenuEntry[] = [];
  menuSections: MenuSection[] = [];

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

      const pathPrefix = (sub.path || '').split('-')[0].toLowerCase();
      const accessKey = ((sub.group || pathPrefix || '').toLowerCase() === 'audit') ? 'admin' : pathPrefix;

      if (!accessKey) {
        return false;
      }

      if (accessKey === 'aesthetics') {
        return allowedRoles.has('aesthetics') || allowedRoles.has('laser');
      }

      return allowedRoles.has(accessKey) || allowedRoles.has(accessKey.endsWith('s') ? accessKey.slice(0, -1) : `${accessKey}s`);
    });

    return this.processSubItems(roleFiltered);
  }

  // Display order and metadata for the report sidebar groups.
  // The key is matched against the lowercased first segment of each subItem.path
  // (e.g. "aesthetics-consultations-report" → "aesthetics").
  private static readonly reportGroupMeta: Record<string, { name: string; icon: string; order: number }> = {
    aesthetics: { name: 'Aesthetics', icon: 'face', order: 10 },
    accounting: { name: 'Accounting', icon: 'account_balance', order: 20 },
    audit: { name: 'Audit', icon: 'rule', order: 25 },
    admin: { name: 'Admin', icon: 'admin_panel_settings', order: 30 },
    billing: { name: 'Billing', icon: 'payments', order: 40 },
    dental: { name: 'Dental', icon: 'medical_services', order: 50 },
    employees: { name: 'Employees', icon: 'badge', order: 60 },
    frontdesk: { name: 'Frontdesk', icon: 'folder_shared', order: 70 },
    laser: { name: 'Laser', icon: 'bolt', order: 80 },
    spa: { name: 'Spa', icon: 'spa', order: 90 },
    'staff-roster': { name: 'Staff Roster', icon: 'event_available', order: 100 },
    clockin: { name: 'Clock-In', icon: 'schedule', order: 110 }
  };

  // Bucket the report subItems into per-module/role groups, using the explicit
  // `group` field if provided, otherwise deriving the key from the first segment
  // of the path. Unmatched items are placed in an "Other" bucket at the end.
  // Items inside each group are sorted ascending (alphabetically by label).
  private groupReportSubItems(subItems: SubNavItem[]): SubNavGroup[] {
    const buckets = new Map<string, SubNavItem[]>();

    for (const sub of subItems || []) {
      const rawKey = (sub.group || (sub.path || '').split('-')[0] || '').toLowerCase();
      const key = rawKey || 'other';
      if (!buckets.has(key)) {
        buckets.set(key, []);
      }
      buckets.get(key)!.push(sub);
    }

    const groups: SubNavGroup[] = [];
    for (const [key, items] of buckets.entries()) {
      const meta = MainLayoutComponent.reportGroupMeta[key] || {
        name: this.toTitleCase(key),
        icon: 'description',
        order: 999
      };
      const sortedItems = items
        .slice()
        .sort((a, b) => (a.label || '').localeCompare(b.label || ''));
      groups.push({ name: meta.name, icon: meta.icon, items: sortedItems });
    }

    groups.sort((a, b) => {
      const orderA = MainLayoutComponent.reportGroupMeta[this.findKeyForName(a.name)]?.order ?? 999;
      const orderB = MainLayoutComponent.reportGroupMeta[this.findKeyForName(b.name)]?.order ?? 999;
      if (orderA !== orderB) return orderA - orderB;
      return a.name.localeCompare(b.name);
    });

    return groups;
  }

  private findKeyForName(name: string): string {
    const entry = Object.entries(MainLayoutComponent.reportGroupMeta)
      .find(([, meta]) => meta.name === name);
    return entry ? entry[0] : '';
  }

  private toTitleCase(value: string): string {
    if (!value) return 'Other';
    return value
      .split(/[-_\s]+/)
      .filter(Boolean)
      .map(part => part.charAt(0).toUpperCase() + part.slice(1))
      .join(' ');
  }

  private normalizeSidebarKey(value: string | undefined): string {
    return (value || '').trim().toLowerCase();
  }

  private resolveSidebarColor(primaryKey: string | undefined, fallbackKey?: string): string {
    const normalizedPrimary = this.normalizeSidebarKey(primaryKey);
    const normalizedFallback = this.normalizeSidebarKey(fallbackKey);

    if (normalizedPrimary && MainLayoutComponent.sidebarIconPalette[normalizedPrimary]) {
      return MainLayoutComponent.sidebarIconPalette[normalizedPrimary];
    }

    if (normalizedFallback && MainLayoutComponent.sidebarIconPalette[normalizedFallback]) {
      return MainLayoutComponent.sidebarIconPalette[normalizedFallback];
    }

    return '#546e7a';
  }

  getMenuIconColor(title: string, route?: string): string {
    return this.resolveSidebarColor(route, title);
  }

  getSubItemIconColor(parentTitle: string, parentRoute?: string): string {
    return this.resolveSidebarColor(parentRoute, parentTitle);
  }

  getGroupIconColor(groupName: string): string {
    return this.resolveSidebarColor(groupName, groupName);
  }

  // Normalize a sub-item label to Title Case (Pascal Case per word).
  // Keeps existing acronyms (e.g. "PDF", "URL") when fully uppercase, and
  // lowercases the rest of each word before re-capitalizing the first letter.
  toTitleCaseLabel(label: string): string {
    if (!label) return '';
    return label
      .split(/\s+/)
      .filter(Boolean)
      .map(word => {
        if (word === word.toUpperCase() && word.length > 1) {
          return word; // keep acronyms as-is
        }
        const cleaned = word.toLowerCase();
        return cleaned.charAt(0).toUpperCase() + cleaned.slice(1);
      })
      .join(' ');
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

    this.http.get<{ Static_Top?: Record<string, NavigationItem>; Dynamic_Roles?: Record<string, NavigationItem>; Static_Bottom?: Record<string, NavigationItem> }>('assets/navigation.json')
      .subscribe(json => {
        // Canonical navigation lives at public/assets/navigation.json (served at /assets/navigation.json).
        const top = this.sortMenuEntries(
          Object.entries(json.Static_Top || {})
            .map(([title, item]) => ({ title, item: { ...item, subItems: this.processSubItems(item.subItems || []) } }))
            .filter(entry => entry.item.visible !== false)
        );

        const dynamic = this.sortMenuEntries(
          Object.entries(json.Dynamic_Roles || {})
            .map(([title, item]) => ({ title, item: { ...item, subItems: this.processSubItems(item.subItems || []) } }))
            .filter(entry => entry.item.visible !== false && this.canAccessDynamicSection(entry.title) && (entry.item.subItems?.length || 0) > 0)
        );

        const bottom = this.sortMenuEntries(
          Object.entries(json.Static_Bottom || {})
            .map(([title, item]) => {
              if (title === 'Reports') {
                const normalized: SubNavItem[] = (item.subItems || []).map(s => ({
                  ...s,
                  label: this.toTitleCaseLabel(s.label || '')
                }));
                const filtered = this.filterReportSubItems(normalized);
                const subGroups = this.groupReportSubItems(filtered);
                return {
                  title,
                  item: { ...item, subItems: filtered },
                  subGroups: subGroups.length > 1 ? subGroups : undefined
                };
              }

              return {
                title,
                item: { ...item, subItems: this.processSubItems(item.subItems || []) }
              };
            })
            .filter(entry => entry.item.visible !== false)
            .filter(entry => {
              if (entry.title === 'Admin') {
                return this.canAccessDynamicSection(entry.title) && (entry.item.subItems?.length || 0) > 0;
              }

              return (entry.item.subItems?.length || 0) > 0;
            })
        );

        this.menuEntries = [...top, ...dynamic, ...bottom];
        const sections: MenuSection[] = [
          { key: 'top', entries: top },
          { key: 'dynamic', entries: dynamic },
          { key: 'bottom', entries: bottom }
        ];
        this.menuSections = sections.filter(section => section.entries.length > 0);
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

  private sortMenuEntries<T extends MenuEntry>(entries: T[]): T[] {
    return entries.slice().sort((a, b) => {
      const sa = typeof a.item.serialNo === 'number' ? a.item.serialNo : 0;
      const sb = typeof b.item.serialNo === 'number' ? b.item.serialNo : 0;
      if (sa !== sb) return sa - sb;
      return a.title.localeCompare(b.title);
    });
  }
}
