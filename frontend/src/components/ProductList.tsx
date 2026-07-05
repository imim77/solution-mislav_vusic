import { Link } from 'react-router-dom'
import type {Product} from '../models/Products'

type ProductListProps = {
    products: Product[];
}

export default function ProductCard({products}: ProductListProps) {
  if (products.length === 0) return null

  return (
    <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      {products.map((product) => (
        <Link
          key={`${product.title}-${product.thumbnail}`}
          to={`/products/${product.id}`}
          className="flex flex-col overflow-hidden rounded-lg bg-greyscale-200"
        >
          <div className="aspect-square bg-greyscale-100">
            <img
              src={product.thumbnail}
              alt={product.title}
              className="h-full w-full object-cover"
            />
          </div>
          <div className="flex flex-1 flex-col gap-2 p-4">
            <h2 className="font-display text-base font-semibold leading-snug line-clamp-1">
              {product.title}
            </h2>
            <p className="text-sm leading-relaxed text-greyscale-500 line-clamp-2">
              {product.description}
            </p>
            <p className="mt-auto font-mono text-lg font-bold text-accent-700">
              ${product.price}
            </p>
          </div>
        </Link>
      ))}
    </div>
  )
}