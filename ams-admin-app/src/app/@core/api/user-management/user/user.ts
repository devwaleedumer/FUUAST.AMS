export interface User {
  id:number,
  userName : string,
  email : string,
  role : string,
  isActive : boolean,
  isEmailConfirmed : boolean,
}

export interface UserRequest{
  id:number,
  userName : string,
  email:string,
  role:string,
}
