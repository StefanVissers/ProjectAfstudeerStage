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
    public project: any;
    public users: any[];
    public projectForm: FormGroup;
    public usersToBeAdded: any[];
    public rolesToBeAdded: string[];

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });
    }

    ngOnInit() {
        this.projectForm = this.formbuilder.group({
            id: [''],
            description: [''],
            isCompleted: [''],
        });

        this.loadSettings();
    }

    // Load Everything from Database.
    loadSettings() {
        // Load project
        this.http.get<any>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            this.projectForm.patchValue(this.project);
        }, error => console.error(error));

        // Load users in a project
        this.http.get<any[]>(this.baseUrl + 'api/Project/Users/' + this.projectId).subscribe(result => {
            this.users = result;
        }, error => console.error(error));

        // Load all users
        this.http.get<any[]>(this.baseUrl + 'api/User').subscribe(result => {
            this.usersToBeAdded = result;
        })

        // Load all roles
        this.http.get<string[]>(this.baseUrl + 'api/Project/UserRoles').subscribe(result => {
            this.rolesToBeAdded = result;
        })
    }

    onFormSubmit() {
        // Update Misc stuff
        this.project.description = this.projectForm.value.description;
        this.project.isCompleted = this.projectForm.value.isCompleted;

        this.http.put<any>(this.baseUrl + 'api/Project/' + this.projectId, this.project).subscribe(result => {
            this.project = result;
            // Update Users
            this.http.put<any>(this.baseUrl + 'api/Project/Users/' + this.projectId, this.users).subscribe(result => {
                this.project = result;
            }, error => console.error(error));
        }, error => console.error(error));
    }

    // Section Users Table
    remove(id: any) {
        this.users.splice(id, 1);
    }

    add() {
        if (this.usersToBeAdded[0]) {
            this.users.push({ name: this.usersToBeAdded[0].username, role: this.rolesToBeAdded[0], userId: this.usersToBeAdded[0].id });
        }
    }

    // Updates the users list when another value is selected.
    changeValue(id: string, property: string, event: any) {
        let x = this.users[id];
        let y = this.usersToBeAdded.filter(z => z.id === event.target.value as string).pop();

        if (property === "name") {
            x.name = y.username;
            x.userId = event.target.value;
        } else {
            x.role = event.target.value;
        }
    }
}
