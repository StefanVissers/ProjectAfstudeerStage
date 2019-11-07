import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    _baseUrl: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
    }

    login(username: string, password: string) {
        return this.http.post<User>(this._baseUrl + 'api/user/authenticate', { username, password })
            .pipe(map(user => {
                // the backend gives us a bearer token in a cookie.
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        // TODO: remove auth cookie and invalidate token.
    }
}
