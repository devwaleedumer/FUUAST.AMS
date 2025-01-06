import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { pagenotfoundComponent } from './pagenotfound.component';


@NgModule({
    imports: [RouterModule.forChild([
        { path: '', component:pagenotfoundComponent }
    ])],
    exports: [RouterModule]
})
export class AccessRoutingModule { }
