import { Component, ElementRef, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import {LayoutService} from '../../@core/services/layout/layout.service';
import { OverlayPanel } from 'primeng/overlaypanel';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  items!: MenuItem[];

  @ViewChild('menubutton') menuButton!: ElementRef;
  @ViewChild('profileMenu') profileMenu!: OverlayPanel;

  @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

  @ViewChild('topbarmenu') menu!: ElementRef;
  constructor(public layoutService: LayoutService,private route:Router) { }

  logout(){
    localStorage.removeItem('token');
    this.route.navigateByUrl('/auth')
  }

  toggleProfileMenu(event: Event) {
    this.profileMenu.toggle(event);
  }
}
