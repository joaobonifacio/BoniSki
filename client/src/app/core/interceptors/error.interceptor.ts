import { Injectable } from '@angular/core';
import { HttpRequest,HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse, HttpParams} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private matSnackBar: MatSnackBar
    //private toastr: ToastrService
    ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error){

          if(error.status === 400){

            if(error.error.errors) {
              //Erros de form, p explo
              //O componente é que lida com a response que obtivemos
              throw error.error;
            }
            else{
              this.matSnackBar.open('Error message ' + error.message + 
              'Error status ' + error.status.toString(), 'Close', { 
                horizontalPosition: 'left',
                duration: 5000 });
            }
          };

          if(error.status === 401){
            this.matSnackBar.open('Error message ' + error.message + 
              'Error status ' + error.status.toString(), 'Close', { 
                horizontalPosition: 'left',
                duration: 5000 });
          };

          if(error.status === 404){
            this.router.navigateByUrl('/not-found');
          };

          if(error.status === 500){
            
            const navigationExtras : NavigationExtras = { state: {error: error.error} }

            this.router.navigateByUrl('/server-error', navigationExtras);
          };
        }
        return throwError(() => new Error(error.message))
      })
      );
    }

}
