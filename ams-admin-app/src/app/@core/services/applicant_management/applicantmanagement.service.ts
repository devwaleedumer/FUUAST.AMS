import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { ApplicantInfoRequest } from '../../api/applicant_management/applicantmanagement';
import { UpdateApplicantRequest } from '../../api/applicant_management/updateapplicantmanagement';

@Injectable({
  providedIn: 'root'
})
export class ApplicantmanagementService {

  _baseEndPoint: string = 'ApplicantManagement';
  constructor(private _httpService: GenericHttpClientService) {
   }
  getAllApplicantDetail(request:ApplicantInfoRequest): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/GetAllApplicantDetails`,undefined,request)
  }
  updateApplicantDetail(request:UpdateApplicantRequest): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/UpdateApplicantDetails`,request)
  }
}
