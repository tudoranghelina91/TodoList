export interface User
{
    id : number;
    email : string;
    hashedPassword : string;
    accessToken : string;
}

export class User
{
    id : number;
    email : string;
    hashedPassword : string;
    accessToken : string;
}