import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { SessionRequest } from '../../api/configuration/session/sessionrequest';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  _baseEndPoint: string = 'Session';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllSession(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}`)
  }
  addSession(request:SessionRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/AddSession`,undefined,request)
    }
    deleteSession(id:number): Observable<any> {
      debugger
      return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteSession?id=${encodeURIComponent(id)}`)
  }
  getAllSessionByFilter (data:any) : Observable<FilterResponse<any>> {
    return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
  }
  updateSession(request:SessionRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateSession`,request)
    }
}
