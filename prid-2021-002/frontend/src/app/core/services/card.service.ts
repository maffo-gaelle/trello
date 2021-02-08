import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { Board } from "../models/board";
import { Card } from "../models/card";
import { List } from "../models/list";
import { User } from "../models/User";

@Injectable({ providedIn: 'root' })
export class CardService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    create(card: Card): Observable<boolean> {
        return this.http.post<Card>(`${this.baseUrl}cards`, card).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public update(card: Card) {
      console.log(card.cardId);
      console.log(card.title);
      return this.http.put<Card>(`${this.baseUrl}cards/${card.cardId}`, card).pipe(
          map(res => true),
          catchError(err => {
              console.error(err);
              return of(false);
          })
      );
  }

    delete(card: Card): Observable<Boolean> {
      return this.http.delete<Card>(`${this.baseUrl}cards/${card.cardId}`).pipe(
          map(res => true),
          catchError(err => {
              console.error(err);
              return of(false);
          })
      );
  }

  changeCard(board: Board, card: Card, oldList: List, newList: List) {
    return this.http.get<Board>(`${this.baseUrl}boards/changeCardOfList/${board.boardId}/${card.cardId}/${oldList.listId}/${newList.listId}`).pipe(
      map(res => true),
      catchError(err => {
          console.error(err);
          return of(false);
      })
    );
  }

  addParticipant(card: Card, user: User, add: boolean)
  {
    return this.http.get(`${this.baseUrl}boards/addOrRemoveParticipants/${card.cardId}/${user.userId}/${add}`).pipe();
  }
  public cardExists(title: string, id: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}cards/availableTitleCard/${title}/${id}`);
  }

  public changePositionCard(list: List, previous: number, current: number) {

    return this.http.put<any>(`${this.baseUrl}lists/changePositionCard/${list.listId}/${previous}/${current}`, list);
  }

  // public getListTitle(card: Card) {
  //   return this.http.get(`${this.baseUrl}cards/getListTitle/${card.cardId}`);
  // }
  public getList(card: Card) {
    return this.http.get<List>(`${this.baseUrl}cards/getList/${card.cardId}`).pipe(
        map((b: any) => {
            return new Board(b);
        }),
        catchError(err => of(null))
        );
  }
}


