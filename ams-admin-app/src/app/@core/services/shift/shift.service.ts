import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { ShiftRequest } from '../../api/configuration/shift/shift';
import { FilterResponse } from '../../api/filter-response';


@Injectable({
  providedIn: 'root'
})

export class ShiftService {
  _baseEndPoint: string = 'Shift';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllShift(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}/GetAllShift`)
  }
 addShift(request:ShiftRequest): Observable<any> {
  return this._httpService.post<any>(`${this._baseEndPoint}/AddShift`,undefined,request)
  }
  deleteShift(id:number): Observable<any> {
    debugger
    return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteShift?id=${encodeURIComponent(id)}`)
}
updateShift(request:ShiftRequest): Observable<any> {
  return this._httpService.put<any>(`${this._baseEndPoint}/updateShift`,request)
  }
  getAllShiftByFilter (data:any) : Observable<FilterResponse<any>> {
    return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
  }
}

