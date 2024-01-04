import { OrderDirection } from './core/order-direction.enum';

export interface SearchRequest<TParameters> {
  page: number;
  pageSize: number;
  orderProperty?: string;
  orderDirection?: OrderDirection;
  parameters?: TParameters;
}
