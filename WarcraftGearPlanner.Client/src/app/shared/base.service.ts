import { HttpClient, HttpParams } from '@angular/common/http';
import { map, switchMap } from 'rxjs';
import { CacheService } from './cache/cache.service';
import { ConfigService } from './config/config.service';

export abstract class BaseService {
  protected abstract _appName: string;

  private _baseUrl$ = this._configService.config$.pipe(
    map((config) => config.baseUrls[this._appName])
  );

  constructor(
    private _http: HttpClient,
    private _configService: ConfigService,
    protected cacheService: CacheService
  ) {}

  protected get<TReturns>(route: string, params?: HttpParams) {
    return this._baseUrl$.pipe(
      map((baseUrl) => `${baseUrl}/api/${route}`),
      switchMap((url) => this._http.get<TReturns>(url, { params }))
    );
  }

  protected post<TReturns, TBody>(
    route: string,
    body: TBody,
    params?: HttpParams
  ) {
    return this._baseUrl$.pipe(
      map((baseUrl) => `${baseUrl}/api/${route}`),
      switchMap((url) => this._http.post<TReturns>(url, body, { params }))
    );
  }
}
