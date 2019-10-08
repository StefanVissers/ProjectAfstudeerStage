import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service'

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private cookieService: CookieService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with basic auth credentials if available
        let cookieValue = this.cookieService.get('Auth');
        //let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (cookieValue) {
            request = request.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + cookieValue
                }
            });
        }

        return next.handle(request);
    }
}
