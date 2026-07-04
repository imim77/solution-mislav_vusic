import type {Product} from '../models/Products'

type ProductListProps = {
    products: Product[];
}


export default function ProductCard({products}: ProductListProps) {
  return (
    <div className='flex flex-row flex-wrap gap-4'>
    {products.map((product) => (
      <div className='flex w-[250px] flex-col gap-4 rounded-lg bg-greyscale-700 p-4'
        key={`${product.title}-${product.thumbnail}`}
      >
        <img src={product.thumbnail} alt={product.title} className='rounded-md' />
        <div className='flex flex-row justify-between'> 
          <div>
            <h2 className='text-xl font-bold'>{product.title}</h2>
            <p className='opacity-50'>{product.description}</p>
          </div>
          <p>${product.price}</p>
        </div> 
      </div>
    ))}
    </div>
  )
}
