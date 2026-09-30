export type Range = {
  from: number
  to: number
}

/**
 *
 * @param value
 * @param range
 * @returns gt From and le To
 */
export function isInRange(value: number, range: Range): boolean {
  return value > range.from && value <= range.to
}
