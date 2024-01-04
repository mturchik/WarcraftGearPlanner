import { Injectable, isDevMode } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { CacheService } from '../cache/cache.service';
import { Config } from './config.model';

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private _config$: ReplaySubject<Config> = new ReplaySubject<Config>(1);
  public config$ = this._config$.asObservable();

  private get _configFile(): string {
    return isDevMode() ? '/assets/config.dev.json' : '/assets/config.json';
  }

  constructor(private _cacheService: CacheService) {}

  public async loadConfig(): Promise<void> {
    const config = this._cacheService.getItem<Config>('config');
    if (config) {
      this._config$.next(config);
      return;
    }

    const configResponse = await fetch(this._configFile);
    const configData = (await configResponse.json()) as Config;
    console.log('Loaded config for environment: ', configData.environment);

    this._cacheService.setItem('config', configData);
    this._config$.next(configData);
  }
}
