import { useState, useEffect } from 'react'
import { useSearchParams } from 'react-router-dom'
import ProductCard from './components/ProductList'
import { Client } from './Client'
import type { Product } from './models/Products'
import ProductListFilters, { type ProductFilters } from './components/ProductListFilters'
import Navbar from './components/Navbar'
import { paramsToFilters, filtersToParams } from './utils/filters'
import Pagination from './components/Pagination'
import { getPageItems, getTotalPages, getVisibleRange } from './utils/pagination'

const PAGE_SIZE = 8

function App() {
  const [products, setProducts] = useState<Product[]>([])
  const [page, setPage] = useState(1)
  const [searchParams, setSearchParams] = useSearchParams() 
  const filters = paramsToFilters(searchParams)
  const totalPages = getTotalPages(products.length, PAGE_SIZE)
  const visibleProducts = getPageItems(products, page, PAGE_SIZE)
  const visibleRange = getVisibleRange(products.length, page, PAGE_SIZE)

  useEffect(() => {
    const client = new Client()
    const request = filters.slug
      ? client.getProductsByCategory(filters.slug, filters.minPrice, filters.maxPrice)
      : client.searchProducts(filters.search)
    request
      .then(data => setProducts(data))
      .catch(err => console.error(err))
  }, [filters.search, filters.slug, filters.minPrice, filters.maxPrice])

  useEffect(() => {
    if (totalPages > 0 && page > totalPages) {
      setPage(totalPages)
    }
  }, [page, totalPages])

  const handleFiltersChange = (next: ProductFilters) => {
    setPage(1)
    setSearchParams(prev => filtersToParams(next, prev), { replace: false })
  }

  return (
    <div className='flex flex-col gap-4'>
      <Navbar/>
      <ProductListFilters initialFilters={filters} onChange={handleFiltersChange} />
      {products.length > 0 && (
        <p className="text-sm opacity-70">
          Showing {visibleRange.from}-{visibleRange.to} of {products.length} products
        </p>
      )}
      <ProductCard products={visibleProducts} />
      <Pagination
        currentPage={page}
        totalPages={totalPages}
        onPageChange={setPage}
      />
    </div>
  )
}

export default App
