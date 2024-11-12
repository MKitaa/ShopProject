import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {LoginRequest} from '../Interfaces/login-request';
import {LoginResponse} from '../Interfaces/login-response';
import {BehaviorSubject, catchError, map, Observable, of, tap, throwError} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5223';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private httpClient: HttpClient) {
  }

  login(credentials: LoginRequest) {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login?useSessionCookies=true`, credentials, {withCredentials: true});
  }

  register(credentials: LoginRequest): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}/register`, credentials, {withCredentials: true});
  }

  isLoggedIn(): Observable<boolean> {
    return this.httpClient.get(`${this.apiUrl}/api/auth/check-auth`, {withCredentials: true})
      .pipe(
        map(response => {
          this.isAuthenticatedSubject.next(true)
          return true;
        }),
        catchError(error => {
          if (error.status === 401) {
            this.isAuthenticatedSubject.next(false)
            return of(false);
          }
          return throwError(error);
        })
      );
  }

  logout(): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}/api/auth/logout`, {}, {withCredentials: true})
  }

  checkRole() {
    return this.httpClient.get(`${this.apiUrl}/api/auth/check-role`, {withCredentials: true})
  }
}
