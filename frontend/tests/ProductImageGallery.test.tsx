import { cleanup, fireEvent, render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it } from 'vitest'
import ProductImageGallery from '../src/components/ProductImageGallery'

describe('ProductImageGallery', () => {
  afterEach(() => {
    cleanup()
  })

  it('renders a fallback when there are no images', () => {
    render(<ProductImageGallery images={[]} title="Phone" />)

    expect(screen.getByText('No image')).toBeTruthy()
  })

  it('changes the selected image when a thumbnail is clicked', () => {
    render(<ProductImageGallery images={['first.jpg', 'second.jpg']} title="Phone" />)

    expect(screen.getByAltText('Phone').getAttribute('src')).toBe('first.jpg')

    fireEvent.click(screen.getByLabelText('Show image 2 of Phone'))

    expect(screen.getByAltText('Phone').getAttribute('src')).toBe('second.jpg')
  })
})
