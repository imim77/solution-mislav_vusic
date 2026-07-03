import { useState, useEffect } from 'react'
import ProductCard from './components/ProductCard'
import { Client } from './Client'
import type { Product } from './models/Products'

function App() {
  const [products, setProducts] = useState<Product[]>([])

  useEffect(() => {
    const client = new Client()
    client.getProducts()
      .then(data => {
        console.log(data)
        setProducts(data)
      })
      .catch(err => console.error(err))
  }, [])

  return (
    <div>
      {products.map((product, index) => (
        <ProductCard key={index} />
      ))}
    </div>
  )
}

export default App
