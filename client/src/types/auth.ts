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
    refreshToken: string,
    accessToken: string,
    refreshTokenExpiryTime: string
}