import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { Faculty } from '../../api/configuration/faculty/faculty';
import { FilterResponse } from '../../api/filter-response';
import { User } from '../../api/user-management/user/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  _baseEndPoint: string = 'users';
  constructor(private _httpService: GenericHttpClientService) { }
  getAllUsers(): Observable<User[]> {
    return this._httpService.get<User[]>(`${this._baseEndPoint}`)
  }
  getAllUsersByFilter(data: any): Observable<FilterResponse<User>> {
    return this._httpService.post<FilterResponse<User>>(`${this._baseEndPoint}/filter`, undefined, data)
  }
  createUser(data: Partial<User>): Observable<{ message: string }> {
    return this._httpService.post<{ message: string }>(`${this._baseEndPoint}`, undefined, data)
  }
  updateUser(data: Partial<User>, userId: number): Observable<User> {
    return this._httpService.put<User>(`${this._baseEndPoint}/${userId}`, data)
  }
  deleteUser(userId: number): Observable<any> {
    return this._httpService.delete(`${this._baseEndPoint}/${userId}`)
  }
  toggleUserStatus(data: any): Observable<any> {
    return this._httpService.post<any>(`accounts/toggle-status`, data)
  }
}

