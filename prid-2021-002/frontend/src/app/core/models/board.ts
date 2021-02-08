import { List } from "./list";
import { User } from "./User";

export class Board {
    boardId: number;
    title: string;
    timestamp: string;
    picturePath: string;
    author: User;
    authorId: number;
    collaborators: User[];
    lists: List[];

    constructor(data: any) {
        if (data) {
            this.boardId = data.boardId;
            this.title = data.title;
            this.timestamp = data.timestamp;
            this.picturePath = data.picturePath;
            this.author = data.author;
            this.authorId = data.autorId;
            this.collaborators = data.collaborators;
            this.lists = data.lists;
        }
    }
}