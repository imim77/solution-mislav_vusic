import type { ProductReview } from "../models/Products"
import StarRating from "./StarRating"

interface ProductReviewsProps {
  reviews: ProductReview[]
}
 
function ProductReviews({ reviews }: ProductReviewsProps) {
  if (reviews.length === 0) return null
 
  return (
    <div>
      <h2 className="mb-4 font-display text-2xl font-bold tracking-tight">Reviews</h2>
      <div className="flex flex-col gap-4">
        {reviews.map((review, index) => (
          <div
            key={`${review.reviewerName}-${review.date}-${index}`}
            className="rounded-lg bg-greyscale-200 p-4"
          >
            <div className="mb-2 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <StarRating rating={review.rating} />
                <span className="text-sm font-semibold">{review.reviewerName}</span>
              </div>
              <time className="font-mono text-xs text-greyscale-500">
                {new Date(review.date).toLocaleDateString()}
              </time>
            </div>
            <p className="text-sm leading-relaxed text-greyscale-600">{review.comment}</p>
          </div>
        ))}
      </div>
    </div>
  )
}
 
export default ProductReviews
