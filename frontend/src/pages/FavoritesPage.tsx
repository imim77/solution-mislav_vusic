import { useQuery } from '@tanstack/react-query'
import { client } from '../Client'
import Navbar from '../components/Navbar'
import ProductList from '../components/ProductList'
import { getUserId } from '../utils/auth'

function FavoritesPage() {
  const userId = getUserId()

  const { data: favorites, isLoading, isError } = useQuery({
    queryKey: ['favorites', userId],
    queryFn: () => client.getFavorites(userId!),
    enabled: userId != null,
  })

  return (
    <div className="min-h-screen bg-greyscale-50">
      <Navbar />
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <h1 className="mb-8 font-display text-3xl font-bold tracking-tight text-greyscale-900">
          Favorites
        </h1>

        {isLoading && (
          <p className="font-mono text-sm text-greyscale-500">Loading favorites...</p>
        )}

        {isError && (
          <p className="text-sm text-accent-700">Unable to load favorites.</p>
        )}

        {!isLoading && !isError && favorites && favorites.length === 0 && (
          <p className="font-mono text-sm text-greyscale-500">No favorites yet.</p>
        )}

        {!isLoading && !isError && favorites && favorites.length > 0 && (
          <ProductList products={favorites} />
        )}
      </main>
    </div>
  )
}

export default FavoritesPage
