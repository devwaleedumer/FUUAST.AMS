import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { FilterResponse } from '../../api/filter-response';

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

  feeChallanData(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}/fee-challan-revenue`)
  }
  applicationByData(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}/application-by-departments`)
  }
  getAllAuditsByFilter(data: any): Observable<FilterResponse<any>> {
    return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/audit-data`, undefined, data)
  }
}
