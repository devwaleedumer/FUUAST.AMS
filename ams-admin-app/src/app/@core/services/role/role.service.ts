import { Injectable } from '@angular/core';
import { GenericHttpClientService } from '../../utilities/generic-http-client.service';
import { Observable } from 'rxjs';
import { Role } from '../../api/auth/role';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  _baseEndPoint: string = 'roles';
  constructor(private _httpService: GenericHttpClientService) { }
  getAllRoles(): Observable<Role[]> {
    return this._httpService.get<Role[]>(`${this._baseEndPoint}`)
  }
  getAllRolesWithPermissions(): Observable<Role[]> {
    return this._httpService.get<Role[]>(`${this._baseEndPoint}/roles-with-permissions`)
  }
  updateRolePermissions(roleId: number, data: any): Observable<any> {
    return this._httpService.put<any>(`${this._baseEndPoint}/${roleId}/permissions`, data)
  }
  // getAllFacultyByFilter (data:any) : Observable<FilterResponse<Faculty>> {
  //   return this._httpService.post<FilterResponse<Faculty>>(`${this._baseEndPoint}/filter`,undefined,data)
  // }
  createRole(data: Partial<Role>): Observable<any> {
    return this._httpService.post<Role>(`${this._baseEndPoint}`, undefined, data)
  }
  updateRole(data: Partial<Role>, roleId: number): Observable<Role> {
    return this._httpService.put<Role>(`${this._baseEndPoint}/${roleId}`, data)
  }
  deleteRole(roleId: number): Observable<any> {
    return this._httpService.delete(`${this._baseEndPoint}/${roleId}`)
  }
  // Permissions
  getAllPermission(): Observable<string[]> {
    return this._httpService.get<string[]>(`${this._baseEndPoint}/all-permissions`)
  }
}
