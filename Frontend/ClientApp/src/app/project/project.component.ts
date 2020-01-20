import { Component, OnInit, Inject } from '@angular/core';
import { Project } from '../models/project';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-project',
    templateUrl: './project.component.html',
    styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
    public projects: Project[]

    constructor(http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        http.get<Project[]>(baseUrl + 'api/Project/').subscribe(result => {
            this.projects = result;
        }, error => console.error(error));
    }

    ngOnInit() {
    }

    getUrl(id: string) {
        return this.baseUrl + 'project/' + id;
    }

}
