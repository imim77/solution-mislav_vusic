import { useState } from 'react'
import { Link, NavLink, useNavigate } from 'react-router-dom'
import { isAuthenticated } from '../utils/auth'

const links = [
  { to: '/', label: 'Home', requiresAuth: false },
  { to: '/favorites', label: 'Favorites', requiresAuth: true },
]

function Navbar() {
  const [open, setOpen] = useState(false)
  const navigate = useNavigate()

  const handleLinkClick = (e: React.MouseEvent, requiresAuth: boolean) => {
    setOpen(false)
    if (requiresAuth && !isAuthenticated()) {
      e.preventDefault()
      navigate('/login')
    }
  }

  return (
    <nav
      aria-label="Primary"
      className="sticky top-0 z-20 border-b border-greyscale-300 bg-greyscale-50"
    >
      <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6 lg:px-8">
        <Link
          to="/"
          onClick={() => setOpen(false)}
          className="font-display text-xl font-bold tracking-tight text-greyscale-900 sm:text-2xl"
        >
          Abysalto akademija
        </Link>

        <ul className="hidden items-center gap-8 sm:flex">
          {links.map((link) => (
            <li key={link.to}>
              <NavLink
                to={link.to}
                onClick={(e) => handleLinkClick(e, link.requiresAuth)}
                className={({ isActive }) =>
                  `text-sm font-medium ${
                    isActive
                      ? 'text-accent-700'
                      : 'text-greyscale-500 hover:text-greyscale-900'
                  }`
                }
              >
                {link.label}
              </NavLink>
            </li>
          ))}
        </ul>

        <button
          type="button"
          aria-expanded={open}
          aria-controls="navbar-mobile"
          aria-label="Toggle navigation"
          onClick={() => setOpen((prev) => !prev)}
          className="flex h-10 w-10 items-center justify-center rounded-md border border-greyscale-300 text-greyscale-800 sm:hidden"
        >
          {open ? (
            <div className="relative h-5 w-5">
              <span className="absolute top-1/2 left-0 h-0.5 w-5 -translate-y-1/2 rotate-45 bg-current" />
              <span className="absolute top-1/2 left-0 h-0.5 w-5 -translate-y-1/2 -rotate-45 bg-current" />
            </div>
          ) : (
            <div className="flex flex-col gap-1.5">
              <span className="h-0.5 w-5 bg-current" />
              <span className="h-0.5 w-5 bg-current" />
              <span className="h-0.5 w-5 bg-current" />
            </div>
          )}
        </button>
      </div>

      {open && (
        <ul
          id="navbar-mobile"
          className="flex flex-col gap-1 border-t border-greyscale-300 bg-greyscale-50 px-4 py-3 sm:hidden"
        >
          {links.map((link) => (
            <li key={link.to}>
              <NavLink
                to={link.to}
                onClick={(e) => handleLinkClick(e, link.requiresAuth)}
                className={({ isActive }) =>
                  `block rounded-md px-3 py-2.5 text-sm font-medium ${
                    isActive
                      ? 'bg-greyscale-200 text-accent-700'
                      : 'text-greyscale-600 hover:bg-greyscale-100'
                  }`
                }
              >
                {link.label}
              </NavLink>
            </li>
          ))}
        </ul>
      )}
    </nav>
  )
}

export default Navbar