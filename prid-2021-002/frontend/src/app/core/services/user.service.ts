import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { User } from "../models/User";
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from "rxjs";

@Injectable({ providedIn: 'root'})
export class UserService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string
    ) {}

    getCollection() {
        return this.http.get<User[]>(`${this.baseUrl}users`).pipe(
            map(res => res.map(u => new User(u))
            ));
    }

     update(user: User): Observable<any> {
        console.log(user);
        return this.http.put<User>(`${this.baseUrl}users/${user.userId}`, user).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    delete(user: User): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}users/${user.userId}`).pipe(
          map(res => true),
          catchError(err => {
              console.error(err);
              return of(false);
          })
        );
    }

    create(user: User): Observable<boolean> {
        return this.http.post<User>(`${this.baseUrl}users`, user).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public uploadPicture(pseudo: any, file: any): Observable<string> {
        console.log(file);
        const formData = new FormData();
        formData.append('pseudo', pseudo);
        formData.append('picture', file);

        return this.http.post<string>(`${this.baseUrl}users/upload`, formData).pipe(
            catchError(err => {
                return of(null);
            })
        );
    }

    public confirmPicture(pseudo: any, path: any): Observable<string> {
        console.log(pseudo, path);
        return this.http.post<string>(`${this.baseUrl}users/confirm`, { pseudo: pseudo, picturePath: path }).pipe(
            catchError(err => {
                console.error(err);
                return of(null);
            })
        );
    }

    public cancelPicture(path): Observable<string> {
        return this.http.post<string>(`${this.baseUrl}user/cancel`, { picturePath: path }).pipe(
            catchError(err => {
                console.error(err);
                return of(null);
            })
        );
    }

}
