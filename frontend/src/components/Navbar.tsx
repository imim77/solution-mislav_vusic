import { useState } from 'react'
import { Link, NavLink } from 'react-router-dom'

const links = [
  { to: '/', label: 'Home' },
  { to: '/favorites', label: 'Favorites' },
]

function Navbar() {
  const [open, setOpen] = useState(false)

  return (
    <nav
      aria-label="Primary"
      className="sticky top-0 z-20 border-b border-greyscale-300 bg-greyscale-50/90 backdrop-blur"
    >
      <div className="mx-auto flex max-w-5xl items-center justify-between px-4 py-4">
        <Link
          to="/"
          onClick={() => setOpen(false)}
          className="font-display text-2xl font-bold tracking-tight text-greyscale-900"
        >
          Products
        </Link>

        <ul className="hidden items-center gap-8 sm:flex">
          {links.map((link) => (
            <li key={link.to}>
              <NavLink
                to={link.to}
                className={({ isActive }) =>
                  `text-sm font-medium transition-colors ${
                    isActive
                      ? 'text-accent-700'
                      : 'text-greyscale-600 hover:text-greyscale-900'
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
          <span className="sr-only">Menu</span>
          <div className="flex flex-col justify-between gap-1.5">
            <span className={`h-0.5 w-5 bg-current transition-transform ${open ? 'translate-y-2 rotate-45' : ''}`} />
            <span className={`h-0.5 w-5 bg-current transition-opacity ${open ? 'opacity-0' : ''}`} />
            <span className={`h-0.5 w-5 bg-current transition-transform ${open ? '-translate-y-2 -rotate-45' : ''}`} />
          </div>
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
                onClick={() => setOpen(false)}
                className={({ isActive }) =>
                  `block rounded-md px-3 py-2 text-sm font-medium ${
                    isActive
                      ? 'bg-greyscale-700 text-accent-700'
                      : 'text-greyscale-700 hover:bg-greyscale-200'
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