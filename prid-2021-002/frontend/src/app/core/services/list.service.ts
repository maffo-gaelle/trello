import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Board } from '../models/board';
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { List } from '../models/list';

@Injectable({ providedIn: 'root' })
export class ListService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    GetListsByBoard(board: Board) {
      return this.http.get<List[]>(`${this.baseUrl}lists/GetListsByBoard/${board.boardId}`).pipe(map(res => res.map(l => new List(l))))
    }

    create(list: List): Observable<boolean> {
        return this.http.post<List>(`${this.baseUrl}lists`, list).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public update(list: List) {
        console.log(list.listId);
        return this.http.put<List>(`${this.baseUrl}lists/${list.listId}`, list).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    deleteList(list: List): Observable<Boolean> {
        return this.http.delete<List>(`${this.baseUrl}lists/${list.listId}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public listExists(title: string, id: number): Observable<boolean> {
        return this.http.get<boolean>(`${this.baseUrl}lists/availableTitleList/${title}/${id}`);
    }

    public moveRigth(board: Board, list: List, moveRight: Boolean) {
      return this.http.get(`${this.baseUrl}boards/moveRigthOrLeft/${board.boardId}/${list.listId}/${moveRight}`).pipe();
    }

    changePositionList(board: Board, list: List, i: number) {
      return this.http.get(`${this.baseUrl}boards/changePosition/${board.boardId}/${list.listId}/${i}`).pipe();
    }

    changeAllCard(l: List, list: List) {
      return this.http.get(`${this.baseUrl}lists/changeAllCard/${l.listId}/${list.listId}`).pipe();
    }

}
