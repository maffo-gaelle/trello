import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Board } from '../models/board';
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { User } from '../models/User';
import { List } from '../models/list';

@Injectable({ providedIn: 'root' })
export class BoardService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    getAll() {
        return this.http.get<Board[]>(`${this.baseUrl}boards`).pipe(map(res => res.map(m => new Board(m))));
    }

    get(id: number) {
        return this.http.get<Board>(`${this.baseUrl}boards/${id}`).pipe(
            map((b: any) => {
                return new Board(b);
            }),
            catchError(err => of(null))
            );
    }

    getBoards(id: number) {
        return this.http.get<Board[]>(`${this.baseUrl}boards/getBoards/${id}`)
            .pipe(map(res => res.map(b => new Board(b))));
    }

    getCollaboratorBoards(id: number) {
        return this.http.get<Board[]>(`${this.baseUrl}boards/getCollaboratorBoards/${id}`)
            .pipe(map(res => res.map(b => new Board(b))));
    }

    create(board: Board): Observable<boolean> {
        return this.http.post<Board>(`${this.baseUrl}boards`, board).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public update(board: Board) {
        console.log(board.boardId);
        return this.http.put<Board>(`${this.baseUrl}boards/${board.boardId}`, board).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public getUsersNotCollaborators(board: Board) {
      return this.http.get<User[]>(`${this.baseUrl}boards/getUsersNotCollaborators/${board.boardId}`).pipe(
        map(res => res.map(u => new User(u))));
    }

    resetBoard(board: Board) {
      return this.http.get(`${this.baseUrl}boards/reset/${board.boardId}`);
    }

    deleteBoard(board: Board): Observable<Boolean> {
      return this.http.delete<Board>(`${this.baseUrl}boards/${board.boardId}`).pipe(
        map(res => true),
        catchError(err => {
            console.error(err);
            return of(false);
        })
      );
    }

    // public addCollaborator(board: Board, user: User, add : boolean) {
    //   return this.http.put(`${this.baseUrl}boards/addOrRemoveCollaborators/${board.boardId}/${user.userId}/${add}`, board).pipe(
    //     map(res => true),
    //     catchError(err => {
    //         console.error(err);
    //         return of(false);
    //     })
    //   );
    // }

    public addCollaborator(board: Board, user: User, add : boolean) {
            return this.http.get(`${this.baseUrl}boards/addOrRemoveCollaborators/${board.boardId}/${user.userId}/${add}`).pipe();
          }


}


