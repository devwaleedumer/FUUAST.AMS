import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { Toast } from 'primeng/toast';
@Injectable({
  providedIn: 'root'
})
export class AccountService {


  _baseEndPoint: string = 'accounts';
  constructor(private _httpService: GenericHttpClientService) {
  }
  resetPassword(request: any, params: any): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/confirm-mail-set-password?code=${params.token}&userId=${params.userId}`, undefined, request)
  }
  //  addDepartment(request:DepartmentRequest): Observable<any> {
  //    return this._httpService.post<any>(`${this._baseEndPoint}/AddDepartment`,undefined,request)
  //    }
  //    deleteDepartment(id:number): Observable<any> {
  //      debugger
  //      return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteDepartment?id=${encodeURIComponent(id)}`)
  //  }
  //  updateDepartment(request:DepartmentRequest): Observable<any> {
  //    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateDepartment`,request)
  //    }
  //    getAllDepartmentByFilter (data:any) : Observable<FilterResponse<any>> {
  //      return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
  //    }
}
