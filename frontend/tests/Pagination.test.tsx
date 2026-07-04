import { cleanup, fireEvent, render, screen } from '@testing-library/react'
import { afterEach, describe, expect, it, vi } from 'vitest'
import Pagination from '../src/components/Pagination'

describe('Pagination', () => {
  afterEach(() => {
    cleanup()
  })

  it('does not render when there is only one page', () => {
    render(<Pagination currentPage={1} totalPages={1} onPageChange={vi.fn()} />)

    expect(screen.queryByLabelText('Product pages')).toBeNull()
  })

  it('renders page buttons', () => {
    render(<Pagination currentPage={1} totalPages={3} onPageChange={vi.fn()} />)

    expect(screen.getByText('1')).toBeTruthy()
    expect(screen.getByText('2')).toBeTruthy()
    expect(screen.getByText('3')).toBeTruthy()
    expect(screen.queryByText('Previous')).toBeNull()
    expect(screen.queryByText('Next')).toBeNull()
  })

  it('calls onPageChange with the requested page', () => {
    const onPageChange = vi.fn()

    render(<Pagination currentPage={2} totalPages={3} onPageChange={onPageChange} />)

    fireEvent.click(screen.getByText('3'))
    fireEvent.click(screen.getByText('1'))

    expect(onPageChange).toHaveBeenNthCalledWith(1, 3)
    expect(onPageChange).toHaveBeenNthCalledWith(2, 1)
  })
})
