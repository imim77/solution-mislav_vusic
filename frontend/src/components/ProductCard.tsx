import type {Product} from '../models/Products'

function ProductCard({title, price, description, thumbnail}: Product) {
  return (
    <div className="border border-gray-200 rounded-lg p-3 shadow-sm hover:shadow-md transition-shadow bg-white w-72">
      <img src={thumbnail} alt={title} className="w-full h-48 object-cover rounded-md mb-2" />
      <h2 className="text-sm font-semibold text-gray-900 mb-1 truncate">{title}</h2>
      <p className="text-base font-bold text-blue-600 mb-1">${price}</p>
      <p className="text-xs text-gray-600 line-clamp-2">{description}</p>
    </div>
  )
}

export default ProductCard