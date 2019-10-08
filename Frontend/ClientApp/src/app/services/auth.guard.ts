import { Injectable, Inject } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    private _baseUrl: string;
    http: HttpClient
    result: Response;
    constructor(http: HttpClient, private router: Router, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
        this.http = http;
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
        //if (localStorage.getItem('currentUser')) {
            // logged in so return true
        this.http.get<Response>(this._baseUrl + 'api/User/Authenticated').subscribe(result => {
            this.result = result;
        }, error => console.error(error));

        if (this.result.ok) {
            return true
        }

            //return true;
        //}

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
