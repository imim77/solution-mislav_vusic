import { useState } from 'react'
import type { SubmitEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { client } from '../Client'
import { INPUT_CLASS_FULL } from '../constants'

function LoginPage() {
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)

  const handleSubmit = async (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    if (!username.trim() || !password.trim()) return

    setLoading(true)
    setError(null)
    setSuccess(false)

    try {
      await client.login(username, password)
      setSuccess(true)
      navigate('/favorites')
    } catch {
      setError('Login failed. Please check your credentials.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-greyscale-50 px-4">
      <div className="w-full max-w-sm rounded-lg bg-greyscale-200 p-8">
        <h1 className="mb-6 font-display text-2xl font-bold tracking-tight text-greyscale-900">
          Sign in
        </h1>
        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <div className="flex flex-col gap-1.5">
            <label htmlFor="username" className="text-sm font-medium text-greyscale-700">
              Username
            </label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Username"
              autoComplete="username"
              className={INPUT_CLASS_FULL}
            />
          </div>
          <div className="flex flex-col gap-1.5">
            <label htmlFor="password" className="text-sm font-medium text-greyscale-700">
              Password
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              autoComplete="current-password"
              className={INPUT_CLASS_FULL}
            />
          </div>
          {error && (
            <p className="text-sm text-accent-700">{error}</p>
          )}
          {success && (
            <p className="text-sm text-greyscale-600">Login successful.</p>
          )}
          <button
            type="submit"
            disabled={loading}
            className="mt-2 rounded-lg bg-accent-700 px-4 py-3 text-sm font-semibold text-white disabled:opacity-50"
          >
            {loading ? 'Signing in...' : 'Sign in'}
          </button>
        </form>
        <Link
          to="/"
          className="mt-6 block text-center text-sm font-medium text-greyscale-500 hover:text-greyscale-900"
        >
          &larr; Back to products
        </Link>
      </div>
    </div>
  )
}

export default LoginPage
