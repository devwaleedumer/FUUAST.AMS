import { HttpInterceptorFn } from '@angular/common/http';
import { LoadingService } from '../../services/loading/loading.service';
import { finalize } from 'rxjs';
import { inject } from '@angular/core';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loader = inject(LoadingService)
  loader.show();
  return next(req).pipe(
    finalize(() => {
      loader.hide();
    }));
};
