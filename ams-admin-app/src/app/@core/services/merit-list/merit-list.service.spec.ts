import { TestBed } from '@angular/core/testing';

import { MeritListService } from './merit-list.service';

describe('MeritListService', () => {
  let service: MeritListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeritListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
