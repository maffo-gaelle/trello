import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { flatMap, map } from 'rxjs/operators';
import { User } from '../models/User';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  // l'utilisateur couramment connecté (undefined sinon)
  public currentUser: User;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    // au départ on récupère un éventuel utilisateur stocké dans le sessionStorage
    const data = JSON.parse(sessionStorage.getItem('currentUser'));
    this.currentUser = data ? new User(data) : null;
  }

  login(pseudo: string, password: string) {
    return this.http.post<User>(`${this.baseUrl}users/authenticate`, { pseudo, password })
      .pipe(map(user => {
        user = new User(user);
        // login successful if there's a jwt token in the response
        if (user && user.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          sessionStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUser = user;
        }

        return user;
      }));
  }

  signup(user: User) {
    return this.http.post<User>(`${this.baseUrl}users`, user).pipe(
      flatMap(res => this.login(user.pseudo, user.password))
    );
  }

  logout() {
    // remove user from local storage to log user out
    sessionStorage.removeItem('currentUser');
    this.currentUser = null;
  }

  pseudoUsed(pseudo: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}users/availablePseudo/${pseudo}`);
  }

  emailUsed(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}users/availableEmail/${email}`);
  }
}