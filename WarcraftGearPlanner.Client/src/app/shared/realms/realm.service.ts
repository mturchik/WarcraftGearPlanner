import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, tap } from 'rxjs';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { Realm } from './realm.model';

@Injectable({ providedIn: 'root' })
export class RealmService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    cacheService: CacheService
  ) {
    super(http, configService, cacheService);
  }

  getRealms() {
    const cachedValue = this.cacheService.getItem<Realm[]>('realms');
    if (cachedValue) return of(cachedValue);

    return this.get<Realm[]>('realms').pipe(
      map((realms) => realms.sort((a, b) => a.name.localeCompare(b.name))),
      tap((realms) => this.cacheService.setItem('realms', realms))
    );
  }
}
