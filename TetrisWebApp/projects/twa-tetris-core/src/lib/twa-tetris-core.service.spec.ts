import { TestBed } from '@angular/core/testing';

import { TwaTetrisCoreService } from './twa-tetris-core.service';

describe('TwaTetrisCoreService', () => {
  let service: TwaTetrisCoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TwaTetrisCoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
