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
    console.log(data)
    return this._httpService.post<FilterResponse<Faculty>>(`${this._baseEndPoint}/filter`,undefined,data)
  }
}
