import { Link } from 'react-router-dom'

function Navbar() {
  return (
    <div>
      <h1 className='text-4xl font-bold'>Products</h1>
      <Link to="/">Home</Link>
      <Link to="/favorites">Favorites</Link>
    </div>
    

  )
}

export default Navbar
