import type { ProductDetails } from '../models/Products'
import StarRating from './StarRating'
 
interface ProductInfoProps {
  product: ProductDetails
}
 
function ProductInfo({ product }: ProductInfoProps) {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <p className="mb-1 font-mono text-xs uppercase tracking-wider text-greyscale-500">
          {product.category}
        </p>
        <h1 className="font-display text-3xl font-bold tracking-tight text-greyscale-900 sm:text-4xl">
          {product.title}
        </h1>
        {product.brand && (
          <p className="mt-1 text-sm text-greyscale-500">by {product.brand}</p>
        )}
      </div>
 
      <p className="font-mono text-3xl font-bold text-accent-700">${product.price}</p>
 
      <div className="flex items-center gap-3">
        <StarRating rating={product.rating} />
        <span className="font-mono text-sm text-greyscale-500">
          {product.rating.toFixed(1)}
        </span>
      </div>
 
      <div className="flex flex-wrap gap-4 text-sm">
        <div className="rounded-md border border-greyscale-300 bg-greyscale-200 px-3 py-2">
          <span className="text-greyscale-500">Stock: </span>
          <span className="font-mono font-semibold">{product.stock}</span>
        </div>
        <div className="rounded-md border border-greyscale-300 bg-greyscale-200 px-3 py-2">
          <span className="text-greyscale-500">Availability: </span>
          <span className="font-semibold">{product.availabilityStatus}</span>
        </div>
      </div>
 
      <div>
        <h2 className="mb-2 font-display text-lg font-semibold">Description</h2>
        <p className="text-sm leading-relaxed text-greyscale-600">{product.description}</p>
      </div>
 
      {product.tags.length > 0 && (
        <div className="flex flex-wrap gap-2">
          {product.tags.map((tag) => (
            <span
              key={tag}
              className="rounded-full bg-greyscale-200 px-3 py-1 text-xs font-medium text-greyscale-600"
            >
              {tag}
            </span>
          ))}
        </div>
      )}
    </div>
  )
}
 
export default ProductInfo
