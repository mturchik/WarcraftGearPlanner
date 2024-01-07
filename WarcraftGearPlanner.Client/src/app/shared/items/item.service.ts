import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { SearchRequest } from '../search.request';
import { ItemSearchParameters } from './models/item-search-parameters.model';

@Injectable({ providedIn: 'root' })
export class ItemService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(configService: ConfigService, cacheService: CacheService) {
    super(configService, cacheService);
  }

  getTotalItemCount() {
    return this.get<number>(`items/count`);
  }

  searchItems(searchRequest: SearchRequest<ItemSearchParameters>) {
    return this.post<any, SearchRequest<ItemSearchParameters>>(
      `items/search-results`,
      searchRequest
    );
  }
}
