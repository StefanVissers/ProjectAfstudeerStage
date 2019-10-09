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
                // login successful if there's a user in the response
                //if (user) {
                    // store user details and basic auth credentials in local storage 
                    // to keep user logged in between page refreshes
                    //user.authdata = window.btoa(username + ':' + password);
                    //localStorage.setItem('currentUser', JSON.stringify(user.token));
                //}
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }
}
