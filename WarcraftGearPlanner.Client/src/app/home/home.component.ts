import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RealmService } from '../services/realm.service';

@Component({
  selector: 'warcraft-gear-planner-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  realmIndex$ = this._realmService.getRealmIndex();

  constructor(private _realmService: RealmService) {}
}
