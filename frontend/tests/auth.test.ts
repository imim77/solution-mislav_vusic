import { afterEach, describe, expect, it } from 'vitest'
import { isAuthenticated, logout } from '../src/utils/auth'

describe('auth utils', () => {
  afterEach(() => {
    localStorage.clear()
  })

  it('detects when a user is authenticated', () => {
    localStorage.setItem('accessToken', 'token')

    expect(isAuthenticated()).toBe(true)
  })

  it('detects when a user is not authenticated', () => {
    expect(isAuthenticated()).toBe(false)
  })

  it('removes the access token on logout', () => {
    localStorage.setItem('accessToken', 'token')

    logout()

    expect(localStorage.getItem('accessToken')).toBeNull()
  })
})
