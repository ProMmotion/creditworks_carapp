import { describe, expect, it } from 'vitest'
import { isUnique } from './array'

describe('isUnique', () => {
  it('accepts the first occurrence of a value', () => {
    expect(isUnique('brand', 0, ['brand', 'model', 'brand'])).toBe(true)
  })

  it('rejects later occurrences of a value', () => {
    expect(isUnique('brand', 2, ['brand', 'model', 'brand'])).toBe(false)
  })

  it('works with number arrays', () => {
    expect(isUnique(2, 1, [1, 2, 3])).toBe(true)
    expect(isUnique(2, 2, [1, 2, 2])).toBe(false)
  })
})
