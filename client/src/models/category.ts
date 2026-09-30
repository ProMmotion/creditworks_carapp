export type Category = {
  id: number
  name: string
  icon: string
  filters: { weight?: { from?: number; to?: number } }
}
