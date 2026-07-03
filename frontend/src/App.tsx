import { useState, useEffect } from 'react'
import ProductCard from './components/ProductList'
import { Client } from './Client'
import type { Product } from './models/Products'
import ProductListFilters, { type ProductFilters } from './components/ProductListFilters'

function App() {
  const [products, setProducts] = useState<Product[]>([])

  useEffect(() => {
    const client = new Client()
    client.getProducts()
      .then(data => {
        setProducts(data)
      })
      .catch(err => console.error(err))
  }, [])

  const handleFiltersChange = (filters: ProductFilters) => {
    const client = new Client()
    const request = filters.slug
      ? client.getProductsByCategory(filters.slug, filters.minPrice, filters.maxPrice)
      : client.searchProducts(filters.search);
    request
      .then(data => setProducts(data))
      .catch(err => console.error(err))
  };

  return (
    <div>
      <ProductListFilters onChange={handleFiltersChange} />
      <ProductCard products={products} />
    </div>
  )
}

export default App
