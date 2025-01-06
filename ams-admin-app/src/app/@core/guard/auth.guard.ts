import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../utilities/auth-service.service';

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);
  const authService = inject(AuthService);
  authService.initializeTokensFromStorage();
  if (authService.isAuthenticated) {
    return true;
  } else {
    router.navigate(['/auth']);
    return false;
  }
};
