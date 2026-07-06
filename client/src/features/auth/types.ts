export type AuthRole = 'admin' | 'user'

export interface LoginResponse {
  token: string
  expiresAt: string
}

export interface AuthSession {
  token: string
  expiresAt: string
  role: AuthRole
}
