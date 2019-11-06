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
    }

    ngOnInit() {
        this.projectForm = this.formbuilder.group({
            id: [''],
            description: [''],
        });

        this.loadSettings();
    }

    // Load Everything from Database.
    loadSettings() {
        // Load project
        this.http.get<Project>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            this.projectForm.patchValue(this.project);
        }, error => console.error(error));

        // Load users in a project
        this.http.get<UserRole[]>(this.baseUrl + 'api/Project/Users/' + this.projectId).subscribe(result => {
            this.users = result;
        }, error => console.error(error));

        // Load all users
        this.http.get<User[]>(this.baseUrl + 'api/User').subscribe(result => {
            this.usersToBeAdded = result;
        })

        // Load all roles
        this.http.get<string[]>(this.baseUrl + 'api/Project/UserRoles').subscribe(result => {
            this.rolesToBeAdded = result;
        })
    }

    onFormSubmit() {
        // Update Misc stuff
        this.project.Description = this.projectForm.value.description;

        this.http.put<Project>(this.baseUrl + 'api/Project/' + this.projectId, this.project).subscribe(result => {
            this.project = result;
            // Update Users
            this.http.put<Project>(this.baseUrl + 'api/Project/Users/' + this.projectId, this.users).subscribe(result => {
                this.project = result;
            }, error => console.error(error));
        }, error => console.error(error));
    }

    // Section Users Table
    remove(id: any) {
        console.log('remove')
        this.users.splice(id, 1);
    }

    add() {
        console.log('add');
        if (this.usersToBeAdded[0]) {
            this.users.push({ Name: this.usersToBeAdded[0].username, Role: this.rolesToBeAdded[0], UserId: this.usersToBeAdded[0].id });
        }
    }

    // Updates the users list when another value is selected.
    changeValue(id: string, property: string, event: any) {
        let x = this.users[id];
        let y = this.usersToBeAdded.filter(z => z.id === event.target.value as string).pop();

        if (property === "name") {
            x.Name = y.username;
            x.UserId = event.target.value;
        } else {
            x.Role = event.target.value;
        }
    }
}
