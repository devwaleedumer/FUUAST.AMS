import { Injectable } from '@angular/core';
import {GenericHttpClientService} from '../../utilities/generic-http-client.service';
import {Observable} from 'rxjs';
import {Faculty} from '../../api/configuration/faculty/faculty';
import {FilterResponse} from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class FacultyService {
  _baseEndPoint: string = 'faculty';
  constructor(private _httpService: GenericHttpClientService) { }
  getAllFaculty(): Observable<Faculty[]> {
    return this._httpService.get<Faculty[]>(`${this._baseEndPoint}`)
  }
  getAllFacultyByFilter (data:any) : Observable<FilterResponse<Faculty>> {
    return this._httpService.post<FilterResponse<Faculty>>(`${this._baseEndPoint}/filter`,undefined,data)
  }
  createFaculty(data: Partial<Faculty>): Observable<Faculty>{
    return this._httpService.post<Faculty>(`${this._baseEndPoint}`,undefined,data)
  }
  updateFaculty(data: Partial<Faculty>,facultyId: number): Observable<Faculty>{
    return this._httpService.put<Faculty>(`${this._baseEndPoint}/${facultyId}`,data)
  }
  deleteFaculty(facultyId: number): Observable<any>{
    return this._httpService.delete(`${this._baseEndPoint}/${facultyId}`)
  }
}
