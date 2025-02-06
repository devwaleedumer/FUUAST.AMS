import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class MeritListService {
  _baseEndPoint: string = 'MeritLists';
  constructor(private _httpService: GenericHttpClientService) {
  }

}
