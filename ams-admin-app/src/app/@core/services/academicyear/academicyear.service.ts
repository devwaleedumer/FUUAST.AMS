import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { AcademicyearRequest } from '../../api/configuration/academicyear/academicyearrequest';
import { FilterResponse } from '../../api/filter-response';

@Injectable({
  providedIn: 'root'
})
export class AcademicyearService {

 
  _baseEndPoint: string = 'AcademicYear';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllAcademicyear(): Observable<any> {
    return this._httpService.get<any>(`${this._baseEndPoint}/GetAllAcademicyear`)
  }
  addAcademicyear(request:AcademicyearRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/AddAcademicyear`,undefined,request)
    }
    deleteAcademicyear(id:number): Observable<any> {
      debugger
      return this._httpService.delete<any>(`${this._baseEndPoint}/DeleteAcademicyear?id=${encodeURIComponent(id)}`)
  }
  updateAcademicyear(request:AcademicyearRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateAcademicyear`,request)
    }
    getAllAcademicyearByFilter (data:any) : Observable<FilterResponse<any>> {
      return this._httpService.post<FilterResponse<any>>(`${this._baseEndPoint}/filter`,undefined,data)
    }
  
}
