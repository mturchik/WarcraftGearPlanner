import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RealmService {
  constructor(private _http: HttpClient) {}

  getRealmIndex() {
    return this._http.get<Realm[]>(
      'https://warcraft-gear-planner.azurewebsites.net/api/realms'
    );
  }

  getRealmIndexSignal() {
    const obs = this.getRealmIndex();
    return signal(obs);
  }
}

export interface Realm {
  id: number;
  name: string;
  slug: string;
}
