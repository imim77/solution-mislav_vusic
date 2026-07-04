import { describe, expect, it } from 'vitest'
import { getPageItems, getTotalPages, getVisibleRange } from '../src/utils/pagination'

describe('pagination utils', () => {
  const items = ['one', 'two', 'three', 'four', 'five']

  it('calculates total pages', () => {
    expect(getTotalPages(items.length, 2)).toBe(3)
    expect(getTotalPages(0, 2)).toBe(0)
  })

  it('returns items for the selected page', () => {
    expect(getPageItems(items, 1, 2)).toEqual(['one', 'two'])
    expect(getPageItems(items, 2, 2)).toEqual(['three', 'four'])
    expect(getPageItems(items, 3, 2)).toEqual(['five'])
  })

  it('returns an empty page for invalid input', () => {
    expect(getPageItems(items, 0, 2)).toEqual([])
    expect(getPageItems(items, 1, 0)).toEqual([])
  })

  it('calculates the visible item range', () => {
    expect(getVisibleRange(17, 1, 8)).toEqual({ from: 1, to: 8 })
    expect(getVisibleRange(17, 3, 8)).toEqual({ from: 17, to: 17 })
    expect(getVisibleRange(0, 1, 8)).toEqual({ from: 0, to: 0 })
  })
})
