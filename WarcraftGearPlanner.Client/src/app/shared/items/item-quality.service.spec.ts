import { TestBed } from '@angular/core/testing';

import { ItemQualityService } from './item-quality.service';

describe('ItemQualityService', () => {
  let service: ItemQualityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemQualityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
