export interface FilterRequest {
  first: number,
  rows: number,
  sortOrder: number,
  sortField?: string,
  filters?: any,
  globalFilter: string,
  multiSortMeta?: any
}
