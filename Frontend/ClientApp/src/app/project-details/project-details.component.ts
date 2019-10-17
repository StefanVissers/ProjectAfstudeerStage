import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project } from '../models/project';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-project-details',
    templateUrl: './project-details.component.html',
    styleUrls: ['./project-details.component.css']
})
export class ProjectDetailsComponent implements OnInit {

    private projectId: string;
    private project: Project

    constructor(http: HttpClient, route: ActivatedRoute, @Inject('BASE_URL') baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });

        http.get<Project>(baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            console.log(this.project);
        }, error => console.error(error));
    }

    ngOnInit() {
    }

}
