import { describe, it, expect } from 'vitest'
import { filtersToParams } from '../src/utils/filters'
import type { ProductFilters } from '../src/components/ProductListFilters'

describe('filtersToParams', () => {
  const baseFilters: ProductFilters = {
    search: '',
    slug: '',
    minPrice: null,
    maxPrice: null,
  }

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
})