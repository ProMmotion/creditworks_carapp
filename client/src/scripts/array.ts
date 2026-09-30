export function isUnique<T>(val: T, index: number, arr: T[]) {
  return arr.indexOf(val) === index
}
