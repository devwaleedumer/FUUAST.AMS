import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { ProgramRequest } from '../../api/configuration/program/program';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class ProgramService {
  _baseEndPoint: string = 'Programs';
  constructor(private _httpService: GenericHttpClientService) {
  }
  getAllprogram(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}`)
  }
  getDepartmentsByProgram(programId: number): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}/${programId}/departments`)
  }
  getAllProgramByFilter(data: any): Observable<FilterResponse<any>> {
    return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`, undefined, data)
  }
  addProgram(request: ProgramRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/AddProgram`, undefined, request)
  }
  deleteProgram(id: number): Observable<any> {
    debugger
    return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteProgram?id=${encodeURIComponent(id)}`)
  }
  updateProgram(request: ProgramRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateProgram`, request)
  }
}
