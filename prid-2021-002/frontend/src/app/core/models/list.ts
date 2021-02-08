import { Board } from "./board";
import { Card } from "./card";

export class List {
    listId: number;
    title: string;
    position: number;
    board: Board;
    boardId: number;
    cards: Card[];


    constructor(data: any) {
        if (data) {
            this.listId = data.listId,
            this.title = data.title,
            this.position = data.position,
            this.board = data.board,
            this.boardId = data.boardId,
            this.cards = data.cards

        }
    }
}
