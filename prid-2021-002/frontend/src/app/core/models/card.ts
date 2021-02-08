import { List } from "./list";
import { User } from "./User";

export class Card {
    cardId: number;
    title: string;
    position: number;
    description: string;
    timestamp: string;
    author: User;
    authorId: number;
    list: List;
    listId: number;
    collaborators: User[];
    openInput: boolean;

    constructor(data: any) {
        if (data) {
            this.cardId = data.cardId;
            this.title = data.title;
            this.position = data.position;
            this.description = data.content;
            this.timestamp = data.timestamp;
            this.author = data.author;
            this.author = data.author;
            this.authorId = data.autorId;
            this.list = data.list;
            this.listId = data.listId;
            this.collaborators = data.collaborators;
            this.openInput = data.openInput;
            //this.collaborators = data.collaborators;

        }
    }
}
