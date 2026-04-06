import { Component, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatExpansionModule } from '@angular/material/expansion';
import { CommonModule } from '@angular/common';

interface NavigationItem {
  route?: string;
  icon?: string;
  subItems?: Array<{ path: string; label: string }>;
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatExpansionModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  menuEntries: Array<{ title: string; item: NavigationItem }> = [];

  get userRoles(): string[] {
    return this.authService.currentUser?.roles || [];
  }

  isSidebarCollapsed = false;

  get fullName(): string {
    return this.authService.currentUser?.fullName || 'User';
  }

  ngOnInit(): void {
    this.http.get<{ Static_Top?: Record<string, NavigationItem>; Dynamic_Roles?: Record<string, NavigationItem>; Settings?: Record<string, NavigationItem> }>('assets/navigation.json')
      .subscribe(json => {
        const top = Object.entries(json.Static_Top || {}).map(([title, item]) => ({ title, item }));
        const dynamic = Object.entries(json.Dynamic_Roles || {})
          .filter(([roleName]) => this.userRoles.includes(roleName))
          .map(([title, item]) => ({ title, item }));
        const bottom = Object.entries(json.Settings || {}).map(([title, item]) => ({ title, item }));

        this.menuEntries = [...top, ...dynamic, ...bottom];
      });
  }

  toggleSidebar(): void {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  logout() {
    this.authService.logout();
    this.authService.redirectLogoutUser();
  }
}
