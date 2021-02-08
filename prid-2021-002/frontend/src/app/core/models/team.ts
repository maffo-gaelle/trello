import { User } from "./User";

export class Team
{
    teamId: number;
    teamname: string;
    collaborators: User[];

    constructor(data: any)
    {
        if (data)
        {
            this.teamId = data.teamId;
            this.teamname = data.teamname;
            this.collaborators = data.Collaborators;
        }
    }
}