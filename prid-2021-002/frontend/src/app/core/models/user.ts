import { Board } from "./board";
import { Card } from "./card";

export enum Role {
    Member = 0,
    Owner = 1,
    Admin = 2
}

export class User {
    userId: number;
    pseudo: string;
    password: string;
    email: string;
    firstName: string;
    lastName: string;
    birthDate: string;
    picturePath: string;
    role: Role;
    token: string;
    boards: Board[];
    cards: Card[];
    collaboratorsBoards: Board[];

    constructor(data: any) {
        if(data) {
            this.userId = data.userId;
            this.pseudo = data.pseudo;
            this.email = data.email;
            this.firstName = data.firstName;
            this.lastName = data.lastName;
            this.birthDate = data.birthDate &&
                             data.birthDate.length > 10 ? data.birthDate.substring(0, 10) : data.birthDate;
            this.picturePath = data.picturePath;
            this.role = data.role || Role.Member;
            this.token = data.token;
        }
    }

    public get roleAsString(): string {
        return Role[this.role];
    }
}
