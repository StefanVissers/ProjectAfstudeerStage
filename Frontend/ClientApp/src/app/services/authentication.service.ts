import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    _baseUrl: string;
    constructor(private cookieService: CookieService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
    }

    login(username: string, password: string) {
        return this.http.post<any>(this._baseUrl + 'api/user/authenticate', { username, password })
            .pipe(map(user => {
                // the backend gives us a bearer token in a cookie.
                return user;
            }));
    }

    logout() {
        this.cookieService.delete('Auth');
    }
}
