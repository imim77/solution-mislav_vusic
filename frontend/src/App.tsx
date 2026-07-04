import { useCallback, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import ProductCard from './components/ProductList'
import { Client } from './Client'
import ProductListFilters, { type ProductFilters } from './components/ProductListFilters'
import Navbar from './components/Navbar'
import { paramsToFilters, filtersToParams } from './utils/filters'
import Pagination from './components/Pagination'
import { getPageItems, getTotalPages, getVisibleRange } from './utils/pagination'
import { useQuery } from '@tanstack/react-query'
import { productsQueryOptions } from './hooks/productsQueryOptions'

const PAGE_SIZE = 8

function App() {
  const [page, setPage] = useState(1)
  const [searchParams, setSearchParams] = useSearchParams()
  const filters = paramsToFilters(searchParams)
  const productsQuery = useQuery(productsQueryOptions(filters));

  const products = productsQuery.data ?? []
  const totalPages = getTotalPages(products.length, PAGE_SIZE)
  const currentPage = totalPages > 0 ? Math.min(page, totalPages) : 1
  const visibleProducts = getPageItems(products, currentPage, PAGE_SIZE)
  const visibleRange = getVisibleRange(products.length, currentPage, PAGE_SIZE)

  const handleFiltersChange = useCallback((next: ProductFilters) => {
    setPage(1)
    setSearchParams(prev => filtersToParams(next, prev), { replace: false })
  }, [setSearchParams])

  return (
    <div className='flex flex-col gap-4'>
      <Navbar/>
      <ProductListFilters
        key={searchParams.toString()}
        initialFilters={filters}
        onChange={handleFiltersChange}
      />
      {productsQuery.isLoading && (
        <p className="text-sm opacity-70">Loading products...</p>
      )}
      {productsQuery.isError && (
        <p className="text-sm text-red-500">Unable to load products.</p>
      )}
      {!productsQuery.isLoading && products.length > 0 && (
        <p className="text-sm opacity-70">
          Showing {visibleRange.from}-{visibleRange.to} of {products.length} products
        </p>
      )}
      {!productsQuery.isError && <ProductCard products={visibleProducts} />}
      {!productsQuery.isError && (
        <Pagination
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}
    </div>
  )
}

export default App
