import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
    data = false;
    userForm: FormGroup;
    massage: string;
    user: any;
    constructor(private formbuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL')
    private baseUrl: string) {
        this.loadForm();
    }

    ngOnInit() {
        this.userForm = this.formbuilder.group({
            id: ['', [Validators.required]],
            username: [{ disabled: true }, [Validators.required]],
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
        });
    }

    onFormSubmit() {
        console.log("onFormSubmit");
        const user = this.userForm.value;
        console.log(user);
        this.http.put<any>(this.baseUrl + 'api/User', user).subscribe(result => {
            console.log("onFormSubmit2");
        }, error => console.error(error));
    }

    loadForm() {
        this.http.get<any>(this.baseUrl + 'api/User/FromToken').subscribe(result => {
            console.log(result);
            this.user = result;
            this.userForm.patchValue(this.user);
        }, error => console.error(error));
    }
}
