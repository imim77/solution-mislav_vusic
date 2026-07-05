import { describe, it, expect } from 'vitest'
import { filtersToParams, paramsToFilters } from '../src/utils/filters'
import type { ProductFilters } from '../src/components/ProductListFilters'

const baseFilters: ProductFilters = {
  search: '',
  slug: '',
  minPrice: null,
  maxPrice: null,
}

describe('filtersToParams', () => {
  it('omits empty values from params', () => {
    const params = filtersToParams({
      search: '',
      slug: '',
      minPrice: null,
      maxPrice: null,
    })
    expect(params.has('q')).toBe(false)
    expect(params.has('category')).toBe(false)
    expect(params.has('minPrice')).toBe(false)
    expect(params.has('maxPrice')).toBe(false)
  })

  it('preserves existing params when prev is provided', () => {
    const prev = new URLSearchParams('sort=price&page=2')
    const params = filtersToParams({ ...baseFilters, search: 'phone' }, prev)
    expect(params.get('sort')).toBe('price')
    expect(params.get('page')).toBe('2')
    expect(params.get('q')).toBe('phone')
  })

  it('overwrites existing param values from prev', () => {
    const prev = new URLSearchParams('q=old')
    const params = filtersToParams({ ...baseFilters, search: 'new' }, prev)
    expect(params.get('q')).toBe('new')
  })

  it('sets price and category params', () => {
    const params = filtersToParams({
      search: '  candy  ',
      slug: 'groceries',
      minPrice: 5,
      maxPrice: 20,
    })

    expect(params.toString()).toBe('q=candy&category=groceries&minPrice=5&maxPrice=20')
  })

  it('removes stale filter params when values are empty', () => {
    const prev = new URLSearchParams('q=phone&category=tech&minPrice=10&maxPrice=100')
    const params = filtersToParams(baseFilters, prev)

    expect(params.toString()).toBe('')
  })
})

describe('paramsToFilters', () => {
  it('reads filters from search params', () => {
    const filters = paramsToFilters(
      new URLSearchParams('q=phone&category=smartphones&minPrice=10&maxPrice=99')
    )

    expect(filters).toEqual({
      search: 'phone',
      slug: 'smartphones',
      minPrice: 10,
      maxPrice: 99,
    })
  })

  it('falls back to empty filters for missing or invalid values', () => {
    const filters = paramsToFilters(new URLSearchParams('minPrice=abc&maxPrice='))

    expect(filters).toEqual(baseFilters)
  })
})
