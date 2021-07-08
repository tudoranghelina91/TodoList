export interface User
{
    id : number;
    username : string;
    email : string;
    hashedPassword : string;
    salt : string;
    accessToken : string;
    expiresIn : number;
}

export class User
{
    id : number;
    username : string;
    email : string;
    hashedPassword : string;
    salt : string;
    accessToken : string;
    expiresIn : number;
}