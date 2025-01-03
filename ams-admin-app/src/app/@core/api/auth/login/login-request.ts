export interface AuthResponse {
  token: string;
  refreshToken: string;
  refreshTokenExpiryTime: string; // ISO date string
}

export interface LoginCredentials {
  username: string;
  password: string;
}
