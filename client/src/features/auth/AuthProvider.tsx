import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import { loginAsAdmin, loginAsUser } from './authApi'
import { clearSession, loadSession, saveSession } from './authStorage'
import type { AuthRole, AuthSession } from './types'

interface AuthContextValue {
  session: AuthSession | null
  isAuthenticated: boolean
  login: (role: AuthRole, identifier: string, password: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<AuthSession | null>(() => loadSession())

  const login = useCallback(
    async (role: AuthRole, identifier: string, password: string) => {
      const response =
        role === 'admin'
          ? await loginAsAdmin(identifier, password)
          : await loginAsUser(identifier, password)

      const nextSession: AuthSession = {
        token: response.token,
        expiresAt: response.expiresAt,
        role,
      }

      saveSession(nextSession)
      setSession(nextSession)
    },
    [],
  )

  const logout = useCallback(() => {
    clearSession()
    setSession(null)
  }, [])

  const value = useMemo(
    () => ({
      session,
      isAuthenticated: session !== null,
      login,
      logout,
    }),
    [session, login, logout],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider')
  }
  return context
}
