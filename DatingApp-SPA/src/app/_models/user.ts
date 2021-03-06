import { Photo } from './photo';
import { StringifyOptions } from 'querystring';

//and UserforDetailed and ForListDTO
export interface User {
    id: number;
    userName: string;
    knownas: string;
    age: number;
    gender: string;
    created: Date;
    lastactive: Date;
    photoUrl: string;
    city: string;
    country: string;

    //? means 可以有也可以沒有 --> 因為要兼容UserforDetailed
    interests?: string;
    introduction?: string;
    lookingfor?: StringifyOptions;
    photos?: Photo[];
    roles?: string[];

}
