import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DepartmentRequest } from '../../api/configuration/department/department';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  _baseEndPoint: string = 'Departments';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllDepartment(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}`)
  }
  addDepartment(request:DepartmentRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/AddDepartment`,undefined,request)
    }
    deleteDepartment(id:number): Observable<any> {
      debugger
      return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteDepartment?id=${encodeURIComponent(id)}`)
  }
  updateDepartment(request:DepartmentRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateDepartment`,request)
    }
    getAllDepartmentByFilter (data:any) : Observable<FilterResponse<any>> {
      return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
    }
}

