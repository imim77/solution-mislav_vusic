import { useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import { useQuery, useMutation } from '@tanstack/react-query'
import { client } from '../Client'
import Navbar from '../components/Navbar'
import ProductImageGallery from '../components/ProductImageGallery'
import ProductInfo from '../components/ProductInfo'
import ProductReviews from '../components/ProductReviews'
import { isAuthenticated } from '../utils/auth'

function ProductDetailPage() {
  const { id } = useParams<{ id: string }>()
  const productId = Number(id)
  const isValidId = Number.isFinite(productId) && productId > 0
  const [successMessage, setSuccessMessage] = useState<string | null>(null)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  const { data: product, isLoading, isError } = useQuery({
    queryKey: ['product', productId],
    queryFn: () => client.getProductById(productId),
    enabled: isValidId,
  })

  const addFavoriteMutation = useMutation({
    mutationFn: () => {
      if (!product) throw new Error('Product not loaded')
      return client.addFavorite(
        product.id,
        product.title,
        product.price,
        product.description,
        product.thumbnail
      )
    },
    onSuccess: () => {
      setSuccessMessage('Added to favorites!')
      setErrorMessage(null)
    },
    onError: () => {
      setErrorMessage('Failed to add to favorites.')
      setSuccessMessage(null)
    },
  })

  return (
    <div className="min-h-screen bg-greyscale-50">
      <Navbar />
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <Link
          to="/"
          className="mb-6 inline-block text-sm font-medium text-greyscale-500 hover:text-greyscale-900"
        >
          &larr; Back to products
        </Link>

        {!isValidId && (
          <p className="text-sm text-accent-700">Invalid product id.</p>
        )}

        {isValidId && isLoading && (
          <p className="font-mono text-sm text-greyscale-500">Loading product...</p>
        )}

        {isValidId && isError && (
          <p className="text-sm text-accent-700">Unable to load product details.</p>
        )}

        {product && (
          <div className="flex flex-col gap-10">
            <div className="grid gap-8 lg:grid-cols-2">
              <ProductImageGallery images={product.images} title={product.title} />
              <ProductInfo product={product} />
            </div>

            {isAuthenticated() && (
              <div className="flex flex-col gap-3">
                <button
                  type="button"
                  onClick={() => addFavoriteMutation.mutate()}
                  disabled={addFavoriteMutation.isPending}
                  className="rounded-lg bg-accent-700 px-6 py-3 text-sm font-semibold text-white disabled:opacity-50"
                >
                  {addFavoriteMutation.isPending ? 'Adding...' : 'Add to favorites'}
                </button>
                {successMessage && (
                  <p className="text-sm text-greyscale-600">{successMessage}</p>
                )}
                {errorMessage && (
                  <p className="text-sm text-accent-700">{errorMessage}</p>
                )}
              </div>
            )}

            <ProductReviews reviews={product.reviews} />
          </div>
        )}
      </main>
    </div>
  )
}

export default ProductDetailPage