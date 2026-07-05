import Navbar from '../components/Navbar'

function FavoritesPage() {
  return (
    <div className="min-h-screen bg-greyscale-50">
      <Navbar />
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <h1 className="font-display text-3xl font-bold tracking-tight text-greyscale-900">
          Favorites
        </h1>
      </main>
    </div>
  )
}

export default FavoritesPage