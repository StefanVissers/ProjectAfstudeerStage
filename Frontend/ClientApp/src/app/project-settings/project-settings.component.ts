import { Component, OnInit, Inject } from '@angular/core';
import { Project } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { User } from '../models/user';

@Component({
    selector: 'app-project-settings',
    templateUrl: './project-settings.component.html',
    styleUrls: ['./project-settings.component.css']
})
export class ProjectSettingsComponent implements OnInit {
    private projectId: string;
    public project: Project;
    public users: User[];

    constructor(private http: HttpClient,
          route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });
    }

    ngOnInit() {
        this.loadSettings();
    }

    loadSettings() {
        this.http.get<Project>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
        }, error => console.error(error));

        this.http.get<User[]>(this.baseUrl + 'api/Project/Users/' + this.projectId).subscribe(result => {
            this.users = result;
        }, error => console.error(error));
    }

}
