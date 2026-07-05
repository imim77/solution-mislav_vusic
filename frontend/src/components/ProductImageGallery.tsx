import { useState } from 'react'
 
interface ProductImageGalleryProps {
  images: string[]
  title: string
}
 
function ProductImageGallery({ images, title }: ProductImageGalleryProps) {
  const [selectedImage, setSelectedImage] = useState(0)
  const hasImages = images.length > 0
 
  return (
    <div className="flex flex-col gap-4">
      <div className="aspect-square overflow-hidden rounded-lg bg-greyscale-200">
        {hasImages ? (
          <img
            src={images[selectedImage]}
            alt={title}
            className="h-full w-full object-cover"
          />
        ) : (
          <div className="flex h-full items-center justify-center text-greyscale-400">
            No image
          </div>
        )}
      </div>
 
      {images.length > 1 && (
        <div className="flex gap-2 overflow-x-auto">
          {images.map((img, i) => (
            <button
              key={img}
              type="button"
              onClick={() => setSelectedImage(i)}
              aria-label={`Show image ${i + 1} of ${title}`}
              aria-current={i === selectedImage}
              className={`h-16 w-16 flex-shrink-0 overflow-hidden rounded-md border-2 ${
                i === selectedImage ? 'border-accent-700' : 'border-transparent'
              }`}
            >
              <img src={img} alt={`${title} ${i + 1}`} className="h-full w-full object-cover" />
            </button>
          ))}
        </div>
      )}
    </div>
  )
}
 
export default ProductImageGallery
