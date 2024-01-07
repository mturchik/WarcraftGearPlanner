import { HttpParams } from '@angular/common/http';
import axios from 'axios';
import { CacheService } from './cache/cache.service';
import { ConfigService } from './config/config.service';

export abstract class BaseService {
  protected abstract _appName: string;

  private get _config() {
    return this._configService.getConfig();
  }

  constructor(
    private _configService: ConfigService,
    protected cacheService: CacheService
  ) {}

  private async _formatUrl(route: string) {
    const baseUrl = (await this._config).baseUrls[this._appName];
    return `${baseUrl}/api/${route}`;
  }

  protected async get<TReturns>(route: string, params?: HttpParams) {
    const url = await this._formatUrl(route);
    const response = await axios.get<TReturns>(url, { params });
    return response.data;
  }

  protected async post<TReturns, TBody>(
    route: string,
    body: TBody,
    params?: HttpParams
  ) {
    const url = await this._formatUrl(route);
    const response = await axios.post<TReturns>(url, body, { params });
    return response.data;
  }
}
