import { Component } from '@angular/core';
import { LoadingService } from './@core/services/loading/loading.service';
import { ProgressBar } from 'primeng/progressbar';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  loading$;

  constructor(private loader: LoadingService) {
    this.loading$ = this.loader.loading$;
  }

  title = 'ams-admin-app';
}
