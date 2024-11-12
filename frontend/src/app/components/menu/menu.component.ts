import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {MatListItem, MatNavList} from '@angular/material/list';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {MatButton, MatIconButton} from '@angular/material/button';
import {NgForOf} from '@angular/common';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {MatIcon} from '@angular/material/icon';
import {AuthService} from '../../services/auth.service';
import {CheckRoleDirective} from '../../Directives/check-role.directive';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    MatNavList,
    RouterLinkActive,
    RouterLink,
    MatListItem,
    MatButton,
    NgForOf,
    MatMenu,
    MatMenuTrigger,
    MatMenuItem,
    MatIcon,
    MatIconButton,
    CheckRoleDirective,
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
  isAuthenticated = false;


  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private router: Router) {
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Logout error:', err);
      }
    });
  }


  ngOnInit() {
    this.authService.isAuthenticated$.subscribe(value => {
      this.isAuthenticated = value;
    });
  }
}
