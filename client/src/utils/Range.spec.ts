import { describe, expect, it } from 'vitest'
import { isInRange } from './Range'

describe('isInRange', () => {
  const range = { from: 1000, to: 2000 }

  it('excludes the lower bound and includes the upper bound', () => {
    expect(isInRange(1000, range)).toBe(false)
    expect(isInRange(1000.01, range)).toBe(true)
    expect(isInRange(2000, range)).toBe(true)
    expect(isInRange(2000.01, range)).toBe(false)
  })

  it('supports an unbounded upper end', () => {
    expect(isInRange(5000, { from: 0, to: Infinity })).toBe(true)
  })
})
