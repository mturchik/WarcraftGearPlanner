import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CacheService } from 'src/app/shared/cache/cache.service';

@Component({
  selector: 'warcraft-gear-planner-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {
  constructor(private _cacheService: CacheService) {}

  clearCache() {
    this._cacheService.reset();
  }
}
