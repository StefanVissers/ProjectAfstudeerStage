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
    private project: Project;
    public categoryId: any;
    public elementId: any;
    public isCollapsed = false;

    constructor(http: HttpClient, route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
            this.categoryId = event.category;
            this.elementId = event.element;
        });

        http.get<Project>(baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            console.log(this.project);
        }, error => console.error(error));
    }

    ngOnInit() {
    }

    getCategoryUrl(projectId: string, categoryId: string) {
        return this.baseUrl + 'project/' + projectId + '/' + categoryId;
    }

    getElementUrl(projectId: string, categoryId: string, elementId: string) {
        return this.baseUrl + 'project/' + projectId + '/' + categoryId + '/' + elementId;
    }

}
