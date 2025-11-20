import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { CartService } from '../../../core/services/cart.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  currentUser$: Observable<User | null>;
  cartItemCount$: Observable<number>;
  isMenuCollapsed = true;

  constructor(
    public authService: AuthService,
    public cartService: CartService,
    private router: Router
  ) {
    this.currentUser$ = this.authService.currentUser$;
    this.cartItemCount$ = new Observable(observer => {
      const interval = setInterval(() => {
        observer.next(this.cartService.itemCount);
      }, 100);
      return () => clearInterval(interval);
    });
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated) {
      this.cartService.loadCart().subscribe();
    }
  }

  logout(): void {
    this.authService.logout();
  }

  get itemCount(): number {
    return this.cartService.itemCount;
  }
}
