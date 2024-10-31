import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import {LayoutService} from '../../../@core/services/layout/layout.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html'
})
export class AppMenuComponent implements OnInit {

  model: any[] = [];

  constructor(public layoutService: LayoutService) { }

  ngOnInit() {
    this.model = [
      {
        label: 'Home',
        items: [
          { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }
        ]
      },
      {
        label: 'System Administration',
        items: [
          { label: 'User Management', icon: 'pi pi-fw pi-users', routerLink: ['/user/users'] },
          { label: 'Configurations', icon: 'pi pi-fw pi-cog',
            items: [
              { label: 'Academic Year', icon: 'pi pi-fw pi-calendar', routerLink: ['/administration/academicYear'] },
              { label: 'Session', icon: 'pi pi-fw pi-clock', routerLink: ['/administration/session'] },
              { label: 'Faculty', icon: 'pi pi-fw pi-briefcase', routerLink: ['/app/configuration/faculty'] },
              { label: 'Department', icon: 'pi pi-fw pi-building', routerLink: ['/administration/session'] },
              { label: 'Program Type', icon: 'pi pi-fw pi-list', routerLink: ['/administration/session'] },
              { label: 'Programs', icon: 'pi pi-fw pi-list', routerLink: ['/administration/session'] },
              { label: 'Program Departments', icon: 'pi pi-fw pi-list', routerLink: ['/administration/session'] },
              { label: 'Shifts', icon: 'pi pi-fw pi-list', routerLink: ['/administration/session'] },
            ]
          },
        ]
      },
      {
        label: 'Application Management',
        icon: 'pi pi-fw pi-briefcase',
        items: [
          {
            label: 'Landing',
            icon: 'pi pi-fw pi-globe',
            routerLink: ['/landing']
          },
          {
            label: 'Auth',
            icon: 'pi pi-fw pi-user',
            items: [
              {
                label: 'Login',
                icon: 'pi pi-fw pi-sign-in',
                routerLink: ['/auth/login']
              },
              {
                label: 'Error',
                icon: 'pi pi-fw pi-times-circle',
                routerLink: ['/auth/error']
              },
              {
                label: 'Access Denied',
                icon: 'pi pi-fw pi-lock',
                routerLink: ['/auth/access']
              }
            ]
          },
        ]
      },
    ];
  }
}
