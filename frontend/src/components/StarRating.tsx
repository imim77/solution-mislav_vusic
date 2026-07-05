interface StarRatingProps {
  rating: number
}
// Claude Sonnet 5 (medium reasoning)
// Prompt: You are an Recat TypeScript expert. Create me a star component for reviews section of my page.
 
const STARS = [1, 2, 3, 4, 5] as const
 
function StarRating({ rating }: StarRatingProps) {
  const rounded = Math.round(rating)
 
  return (
    <span className="inline-flex gap-0.5" aria-label={`Rating: ${rating} out of 5`}>
      {STARS.map((star) => (
        <svg
          key={star}
          className={`h-4 w-4 ${star <= rounded ? 'text-accent-700' : 'text-greyscale-300'}`}
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
        </svg>
      ))}
    </span>
  )
}
 
export default StarRating
