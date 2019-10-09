import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { from, Observable } from 'rxjs';
import { Register } from "../models/register";
@Injectable({
    providedIn: 'root'
})
export class LoginService {
    Url: string;
    token: string;
    header: any;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        

        const headerSettings: { [name: string]: string | string[]; } = {};
        this.header = new HttpHeaders(headerSettings);
    }
    Login(model: any) {
        console.log("login in login service");
        return this.http.post<any>(this.Url + 'api/User/Login', model, { headers: this.header });
    }
    CreateUser(register: Register) {
        const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
        return this.http.post<Register[]>(this.Url + 'api/User', register, httpOptions)
    }
}  
