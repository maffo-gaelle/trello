import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Team } from '../models/team';
import { User } from '../models/User';
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TeamService
{
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){}

    getAll()
    {
        return this.http.get<Team[]>(`${this.baseUrl}teams`).pipe(map(res => res.map(m => new Team(m))));
    }

    get(id:number)
    {
        return this.http.get<Team>(`${this.baseUrl}teams/${id}`).pipe(
            map((t: any) => {
                return new Team(t);
            }),
            catchError(err => of(null))
            );
    }

    getTeams(id:number)
    {
        return this.http.get<Team[]>(`${this.baseUrl}teams/getTeams/${id}`)
            .pipe(map(res => res.map(t => new Team(t))));
    }

    getUsers(id:number)
    {
        return this.http.get<User[]>(`${this.baseUrl}teams/getUsers/${id}`).pipe(
            map(res=>res.map(u => new User(u))));
    }

    // public getUsersNotMembers(team:Team) {
    //     return this.http.get<User[]>(`${this.baseUrl}teams/getUsersNotCollaborators/${team.teamId}`).pipe(
    //       map(res => res.map(u => new User(u))));
    //   }

    create(team:Team):Observable<boolean>
    {
        return this.http.post<Team>(`${this.baseUrl}teams`,team).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public update(team:Team):Observable<boolean>
    {
        return this.http.put<Team>(`${this.baseUrl}teams/${team.teamId}`,team).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }

    public delete(team:Team):Observable<Boolean>
    {
        return this.http.delete<Team>(`${this.baseUrl}teams/${team.teamId}`).pipe(
            map(res=>true),
            catchError(err=>{
                console.error(err);
                return of(false);
            })
        );
    }

    public addremove(team:Team,user:User,add:Boolean):Observable<boolean>
    {
        return this.http.get(`${this.baseUrl}teams/addOrRemoveCollaborators/${team.teamId}/${user.userId}/${add}`).pipe(
            map(res => true),
            catchError(err => {
                console.error(err);
                return of(false);
            })
        );
    }
}
