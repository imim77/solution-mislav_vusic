import { useState, useEffect } from 'react'
import ProductCard from './components/ProductCard'
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
    client.searchProducts(filters.search)
      .then(data => setProducts(data))
      .catch(err => console.error(err))
  };

  return (
    <div>
      <ProductListFilters onChange={handleFiltersChange} />
      {products.map((product) => (
        <ProductCard key={product.title} {...product} />
      ))}
    </div>
  )
}

export default App
