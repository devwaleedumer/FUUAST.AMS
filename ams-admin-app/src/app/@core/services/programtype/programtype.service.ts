import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { ProgramtypeRequest } from '../../api/configuration/programtype/programtype';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class ProgramtypeService {
  _baseEndPoint: string = 'Programtype';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllprogramtype(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}`)
  }
  addProgramtype(request:ProgramtypeRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/AddProgramtype`,undefined,request)
    }
    deleteProgramtype(id:number): Observable<any> {
      debugger
      return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteProgramtype?id=${encodeURIComponent(id)}`)
  }
  updateProgramtype(request:ProgramtypeRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateProgramtype`,request)
    }
    getAllProgramtypeByFilter (data:any) : Observable<FilterResponse<any>> {
      return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
    }
  
}
