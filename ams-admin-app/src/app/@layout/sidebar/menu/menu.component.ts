import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from '../../../@core/services/layout/layout.service';

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
          {
            label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/applayout']
          }
        ]
      },
      {
        label: 'System Administration',
        items: [
          {
            label: 'User Management', icon: 'pi pi-fw pi-users',
            items: [
              {
                label: 'Users',
                icon: 'pi pi-fw  pi-users',
                routerLink: ['/applayout/app/user-management/users']
              },
              {
                label: 'Roles & Permissions',
                icon: 'pi pi-fw  pi-shield',
                routerLink: ['/applayout/app/user-management/roles']
              },
            ]
          },

        ]
      },
      {
        label: 'Configurations', icon: 'pi pi-fw pi-cog',
        items: [
          { label: 'Academic Year', icon: 'pi pi-fw pi-calendar', routerLink: ['/applayout/app/configuration/academicyear'] },
          { label: 'Session', icon: 'pi pi-fw pi-clock', routerLink: ['/applayout/app/configuration/session'] },
          {
            label: 'Shifts', icon: 'pi pi-map', routerLink: ['/applayout/app/configuration/shift'],
          },
          { label: 'Faculty', icon: 'pi pi-fw pi-briefcase', routerLink: ['/applayout/app/configuration/faculty'] },
          { label: 'Department', icon: 'pi pi-fw pi-building', routerLink: ['/applayout/app/configuration/department'] },
          { label: 'Programs', icon: 'pi pi-graduation-cap', routerLink: ['/applayout/app/configuration/program'] },
          { label: 'Program Type', icon: 'pi pi-fw pi-list', routerLink: ['/applayout/app/configuration/programtype'] },
          { label: 'Program Departments', icon: 'pi pi-building-columns', routerLink: ['/administration/session'] },
        ]
      }
      ,
      {
        label: 'Application Management',
        icon: 'pi pi-fw pi-briefcase',
        items: [
          {
            label: 'Applicant Management',
            icon: 'pi pi-fw pi-globe',
            routerLink: ['/applayout/app/application/applicant']
          },

        ]
      },
      {
        label: 'Merit List Management',
        icon: 'pi pi-search',
        items: [
          {
            label: 'Merit List',
            icon: 'pi pi-list-check',
            routerLink: ['/applayout/app/merit-list']
          },

        ]
      },
      {
        label: 'Audit Trails Management',
        icon: 'pi pi-search',
        items: [
          {
            label: 'Audit Trails',
            icon: 'pi pi-fw pi-globe',
            routerLink: ['/applayout/app/audit']
          },

        ]
      },
    ];
  }
}
