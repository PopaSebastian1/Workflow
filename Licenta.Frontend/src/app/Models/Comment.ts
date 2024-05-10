import { User } from "./User";

export interface Comment {
    id: string;
    message: string;
    createdAt: Date;
    userId: string;
    user: User;
}