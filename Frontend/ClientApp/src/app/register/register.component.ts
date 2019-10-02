import { Component, OnInit } from '@angular/core';
import { LoginService } from '../login.service';
import { Register } from '../register';
import { Observable } from 'rxjs';
import { FormGroup, FormArray, FormBuilder, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    data = false;
    userForm: FormGroup;
    massage: string;
    constructor(private formbuilder: FormBuilder, private loginService: LoginService) { }

    ngOnInit() {
        this.userForm = this.formbuilder.group({
            UserName: ['', [Validators.required]],
            Password: ['', [Validators.required]],
            Email: ['', [Validators.required]],
        });
    }
    onFormSubmit() {
        const user = this.userForm.value;
        this.Createemployee(user);
    }
    Createemployee(register: Register) {
        this.loginService.CreateUser(register).subscribe(
            () => {
                this.data = true;
                this.massage = 'Data saved Successfully';
                this.userForm.reset();
            });
    }
}    
