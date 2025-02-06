import { Injectable } from '@angular/core';
import { AuthService } from './auth-service.service';
import { User } from '../api/auth/user';
import { Permission } from '../api/auth/permission';
// import {Role} from '../api/auth/role';

@Injectable({
  providedIn: 'root'
})

export class RolesAndPermissionService {
  constructor() { }

  public hasRequiredRole(user: User, role: string): boolean {
    return user.role === role;
  }
  public hasRequiredPermission(user: User, p: Permission): boolean {
    return user.permission.some(permission => permission === p)
  }
}
