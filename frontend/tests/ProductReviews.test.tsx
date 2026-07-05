import { cleanup, render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it } from 'vitest'
import ProductReviews from '../src/components/ProductReviews'

describe('ProductReviews', () => {
  afterEach(() => {
    cleanup()
  })

  it('does not render when there are no reviews', () => {
    const { container } = render(<ProductReviews reviews={[]} />)

    expect(container.firstChild).toBeNull()
  })

  it('renders review author, rating and comment', () => {
    render(
      <ProductReviews
        reviews={[
          {
            rating: 4,
            comment: 'Works well.',
            reviewerName: 'Ana',
            date: '2026-01-15T00:00:00.000Z',
          },
        ]}
      />
    )

    expect(screen.getByText('Reviews')).toBeTruthy()
    expect(screen.getByText('Ana')).toBeTruthy()
    expect(screen.getByLabelText('Rating: 4 out of 5')).toBeTruthy()
    expect(screen.getByText('Works well.')).toBeTruthy()
  })
})
