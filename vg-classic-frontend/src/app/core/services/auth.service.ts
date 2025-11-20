import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  User,
  LoginRequest,
  RegisterRequest,
  AuthenticationResult,
  TokenResponse
} from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser$: Observable<User | null>;
  private tokenKey = 'vg_token';
  private userKey = 'vg_user';

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    const storedUser = localStorage.getItem(this.userKey);
    this.currentUserSubject = new BehaviorSubject<User | null>(
      storedUser ? JSON.parse(storedUser) : null
    );
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  public get token(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public get isAuthenticated(): boolean {
    return !!this.token && !!this.currentUserValue;
  }

  public get isAdmin(): boolean {
    return this.currentUserValue?.role === 'Admin' ||
           this.currentUserValue?.role === 'SuperAdmin';
  }

  login(credentials: LoginRequest): Observable<AuthenticationResult> {
    return this.apiService.post<AuthenticationResult>('Authentication/login', credentials)
      .pipe(
        tap(result => {
          if (result.isSuccess && result.token) {
            this.setAuth(result.token, result.userId!);
          }
        })
      );
  }

  register(data: RegisterRequest): Observable<AuthenticationResult> {
    return this.apiService.post<AuthenticationResult>('Authentication/register', data)
      .pipe(
        tap(result => {
          if (result.isSuccess && result.token) {
            this.setAuth(result.token, result.userId!);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  private setAuth(tokenResponse: TokenResponse, userId: string): void {
    localStorage.setItem(this.tokenKey, tokenResponse.accessToken);

    // Decode JWT to get user info (simple base64 decode)
    const payload = this.decodeToken(tokenResponse.accessToken);
    const user: User = {
      id: userId,
      email: payload.email || '',
      firstName: payload.given_name || '',
      lastName: payload.family_name || '',
      role: payload.role || 'Customer'
    };

    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(c => {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
      return JSON.parse(jsonPayload);
    } catch (error) {
      return {};
    }
  }
}
