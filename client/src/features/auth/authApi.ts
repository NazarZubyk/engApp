import type { LoginResponse } from './types'

async function postLogin(
  path: string,
  body: Record<string, string>,
): Promise<LoginResponse> {
  const response = await fetch(path, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    if (response.status === 401) {
      throw new Error('Invalid credentials')
    }
    throw new Error('Login failed')
  }

  return response.json() as Promise<LoginResponse>
}

export function loginAsUser(login: string, password: string) {
  return postLogin('/api/auth/user/login', { login, password })
}

export function loginAsAdmin(username: string, password: string) {
  return postLogin('/api/auth/admin/login', { username, password })
}
