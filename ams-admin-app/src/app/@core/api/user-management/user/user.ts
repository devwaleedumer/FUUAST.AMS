export interface User {
  id:number,
  userName : string,
  email : string,
  role : string,
  isActive : boolean,
  isEmailConfirmed : boolean,
  profilePictureUrl: string
}

export interface UserRequest{
  id:number,
  userName : string,
  email:string,
  role:string,
}
