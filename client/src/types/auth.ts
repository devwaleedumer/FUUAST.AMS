export interface IRegisterRequest {
    fullName: string,
    email: string,
    password: string,
    confirmPassword: string
}

export interface IRegisterResponse {
    message: string
}


export interface ILoginRequest {
    email: string,
    password: string,
}


export interface ILoginResponse {
    refreshTokenExpiryTime: string
}

export interface IUser {
    id: number,
    applicantId: number,
    userName: string,
    fullName: string,
    email: string,
    profilePictureUrl: string,
}