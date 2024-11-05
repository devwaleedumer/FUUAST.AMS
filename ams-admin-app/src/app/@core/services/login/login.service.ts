import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  _baseEndPoint: string = 'Tokens';
  constructor(private _httpService: GenericHttpClientService) {
   }
  authentication(request:any): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/get-token`,undefined,request)
    }   
}
