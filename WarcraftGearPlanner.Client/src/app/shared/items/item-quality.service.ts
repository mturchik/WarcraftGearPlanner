import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, tap } from 'rxjs';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { ItemQuality } from './models/item-quality.model';

@Injectable({ providedIn: 'root' })
export class ItemQualityService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    cacheService: CacheService
  ) {
    super(http, configService, cacheService);
  }

  getItemQualities() {
    const cachedValue =
      this.cacheService.getItem<ItemQuality[]>('item-qualities');
    if (cachedValue) return of(cachedValue);

    return this.get<ItemQuality[]>('item-qualities').pipe(
      map((itemQualities) =>
        itemQualities.sort((a, b) => a.name.localeCompare(b.name))
      ),
      tap((itemQualities) =>
        this.cacheService.setItem('item-qualities', itemQualities)
      )
    );
  }
}
