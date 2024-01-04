export interface SearchResponse<TResults> {
  page: number;
  pageSize: number;
  maxPageSize: number;
  pageCount: number;
  resultCountCapped: boolean;
  results: TResults[];
}
