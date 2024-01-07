import { Injectable, isDevMode } from '@angular/core';
import { CacheService } from '../cache/cache.service';
import { Config } from './config.model';

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private get _configFile(): string {
    return isDevMode() ? '/assets/config.dev.json' : '/assets/config.json';
  }

  constructor(private _cacheService: CacheService) {}

  public async getConfig(): Promise<Config> {
    let config = this._cacheService.getItem<Config>('config');
    if (config) return config;

    const configResponse = await fetch(this._configFile);

    config = (await configResponse.json()) as Config;
    console.log('Loaded config for environment: ', config.environment);

    this._cacheService.setItem('config', config);
    return config;
  }
}
