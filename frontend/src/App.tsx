import { useState, useEffect } from 'react'
import { useSearchParams } from 'react-router-dom'
import ProductCard from './components/ProductList'
import { Client } from './Client'
import type { Product } from './models/Products'
import ProductListFilters, { type ProductFilters } from './components/ProductListFilters'
import Navbar from './components/Navbar'
import { paramsToFilters, filtersToParams } from './utils/filters'

function App() {
  const [products, setProducts] = useState<Product[]>([])
  const [searchParams, setSearchParams] = useSearchParams()
  const filters = paramsToFilters(searchParams)

  useEffect(() => {
    const client = new Client()
    const request = filters.slug
      ? client.getProductsByCategory(filters.slug, filters.minPrice, filters.maxPrice)
      : client.searchProducts(filters.search)
    request
      .then(data => setProducts(data))
      .catch(err => console.error(err))
  }, [filters.search, filters.slug, filters.minPrice, filters.maxPrice])

  const handleFiltersChange = (next: ProductFilters) => {
    setSearchParams(prev => filtersToParams(next, prev), { replace: false })
  }

  return (
    <div className='flex flex-col gap2'>
      <Navbar/>
      <ProductListFilters initialFilters={filters} onChange={handleFiltersChange} />
      <ProductCard products={products} />
    </div>
  )
}

export default App