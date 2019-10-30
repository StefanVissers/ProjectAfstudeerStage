import { Component, OnInit, Inject } from '@angular/core';
import { Project, UserRole } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { User } from '../models/user';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-project-settings',
    templateUrl: './project-settings.component.html',
    styleUrls: ['./project-settings.component.css']
})
export class ProjectSettingsComponent implements OnInit {
    private projectId: string;
    public project: Project;
    public users: UserRole[];
    public projectForm: FormGroup;
    public usersToBeAdded: User[];
    public rolesToBeAdded: string[];

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });

        this.projectForm = this.formbuilder.group({
            id: [''],
            description: [''],
            users: [''],
        });
    }

    ngOnInit() {
        this.loadSettings();
    }
    
    loadSettings() {
        this.http.get<Project>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            this.projectForm.patchValue(this.project);
        }, error => console.error(error));

        this.http.get<UserRole[]>(this.baseUrl + 'api/Project/Users/' + this.projectId).subscribe(result => {
            this.users = result;
        }, error => console.error(error));

        this.http.get<User[]>(this.baseUrl + 'api/User').subscribe(result => {
            this.usersToBeAdded = result;
        })

        this.http.get<string[]>(this.baseUrl + 'api/Project/UserRoles').subscribe(result => {
            this.rolesToBeAdded = result;
        })
    }

    onFormSubmit() {
        console.log(this.projectForm.value);
    }

    editField: string;

    updateList(id: number, property: string, event: any) {
        const editField = event.target.textContent;
        console.log('updateList');
        console.log(editField);
    }

    remove(id: any) {
        console.log('remove')
        this.users.splice(id, 1);
    }

    add() {
        console.log('add');
        this.users.push({ Name: '', Role: '', UserId: '' });
    }
    
    changeValue(id: number, property: string, event: any) {
        this.editField = event.target.textContent;
        console.log(event.target.value);
        console.log('changeValue');
        console.log(this.editField);
    }
}
