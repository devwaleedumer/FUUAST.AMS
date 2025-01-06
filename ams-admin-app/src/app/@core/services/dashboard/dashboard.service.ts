import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  _baseEndPoint: string = 'Dashboard';
    constructor(private _httpService: GenericHttpClientService) {
     }
    getDashboard(): Observable<any> {
      return this._httpService.get<any>(`${this._baseEndPoint}`)
    }
}
