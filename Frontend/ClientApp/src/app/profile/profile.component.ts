import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

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
    updated = false;
    error = false;
    errorText: string;
    constructor(private formbuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL')
    private baseUrl: string) {
        this.loadForm();
    }

    ngOnInit() {
        this.userForm = this.formbuilder.group({
            id: ['', [Validators.required]],
            username: [{ value: '', disabled: true }, [Validators.required]],
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
        });
    }

    onFormSubmit() {
        const user = this.userForm.value;
        user.username = this.user.username
        console.log(user);
        this.http.put<any>(this.baseUrl + 'api/User/' + user.id, user).subscribe(result => {
            this.error = false;
            this.updated = true;
            this.loadForm();
        }, error => {
            console.error(error);
            this.error = true;
            this.errorText = this.getErrorText(error);
        });
    }

    loadForm() {
        this.http.get<any>(this.baseUrl + 'api/User/FromToken').subscribe(result => {
            console.log(result);
            this.error = false;
            this.user = result;
            this.userForm.patchValue(this.user);
        }, error => {
            console.error(error);
            this.error = true;
            this.errorText = this.getErrorText(error);
        });
    }

    getErrorText(error) {
        let text = '';
        if (error.errors.Email) {
                text += 'The email is not a correct email address. ';
        }

        if (error.errors.NewPassword) {
            text += 'The new password should be at least 6 characters long. ';
        }

        if (error.errors.OldPassword) {
            text += 'The old password is incorrect. ';
        }

        return text;
    }
}
