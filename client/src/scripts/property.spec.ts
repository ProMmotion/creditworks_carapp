import { describe, expect, it } from 'vitest'
import { getProperty } from './property'

describe('getProperty', () => {
  it('gets a top-level property', () => {
    expect(getProperty({ name: 'Roadster' }, 'name')).toBe('Roadster')
  })

  it('gets a two-level property', () => {
    expect(getProperty({ owner: { name: 'Alex' } }, 'owner.name')).toBe('Alex')
  })

  it('returns undefined for a missing property', () => {
    expect(getProperty({ owner: {} }, 'owner.name')).toBeUndefined()
  })

  it('returns the input object for an empty path', () => {
    const value = { name: 'Roadster' }
    expect(getProperty(value, '')).toBe(value)
  })

  it('returns the unconsumed nested object for paths with more than two segments', () => {
    expect(getProperty({ owner: { profile: { name: 'Alex' } } }, 'owner.profile.name')).toEqual(
      'Alex',
    )
  })
})
