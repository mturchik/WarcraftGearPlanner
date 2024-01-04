import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { ItemClass } from './models/item-class.model';

@Injectable({ providedIn: 'root' })
export class ItemClassService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    cacheService: CacheService
  ) {
    super(http, configService, cacheService);
  }

  getItemClasses(): Observable<ItemClass[]> {
    const cachedValue = this.cacheService.getItem<any[]>('item-classes');
    if (cachedValue) return of(cachedValue);
    return this.get<ItemClass[]>('item-classes').pipe(
      map((itemClasses) =>
        itemClasses.sort((a, b) => a.name.localeCompare(b.name))
      ),
      tap((itemClasses) =>
        this.cacheService.setItem('item-classes', itemClasses)
      )
    );
  }
}
