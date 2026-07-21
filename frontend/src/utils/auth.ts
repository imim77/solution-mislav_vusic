const ACCESS_TOKEN_KEY = 'accessToken'
const USER_ID_KEY = 'userId'

export function isAuthenticated(): boolean {
  return !!localStorage.getItem(ACCESS_TOKEN_KEY)
}

export function getUserId(): number | null {
  const stored = localStorage.getItem(USER_ID_KEY)
  if (stored === null) return null

  const userId = Number(stored)
  return Number.isFinite(userId) ? userId : null
}

export function saveAuthSession(accessToken: string, userId: number): void {
  if (accessToken) {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken)
  }
  if (userId != null) {
    localStorage.setItem(USER_ID_KEY, String(userId))
  }
}

export function logout(): void {
  localStorage.removeItem(ACCESS_TOKEN_KEY)
  localStorage.removeItem(USER_ID_KEY)
}
