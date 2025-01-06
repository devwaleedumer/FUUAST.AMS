import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-access',
    templateUrl: './pagenotfound.component.html',
})
export class pagenotfoundComponent { 

    constructor(private route:Router){

    }

    redirect(){
        this.route.navigateByUrl('/applayout');
    }
}
