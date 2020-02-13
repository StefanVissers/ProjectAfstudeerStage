import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project, WorkflowElementCategory, WorkflowElement } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-project-wrapper',
    templateUrl: './project-wrapper.component.html',
    styleUrls: ['./project-wrapper.component.css']
})
export class ProjectWrapperComponent implements OnInit {
    private projectId: string;
    public project: Project;
    public category: WorkflowElementCategory;
    public element: WorkflowElement;
    public projectForm: FormGroup;
    public categoryId: string;
    public elementId: string;
    public isCollapsed = false;
    public settings = false;
    public ssllabs = false;
    public tooling = false;

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        private route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        this.route.params.subscribe(event => {
            this.projectId = event.id;
            this.categoryId = event.category;
            this.elementId = event.element;
        });
        this.route.data.subscribe(data => {
            this.settings = data.settings;
            this.ssllabs = data.ssllabs;
            this.tooling = data.tooling;
        })
    }

    ngOnInit() {
        this.route.params.subscribe(_ => {
            this.loadElement();
        });
    }

    loadElement() {
        this.http.get<Project>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
        }, error => console.error(error));

        if (this.categoryId) {
            this.http.get<WorkflowElementCategory>(this.baseUrl + 'api/Project/' + this.projectId + '/' + this.categoryId).subscribe(result => {
                this.category = result;
            }, error => console.error(error));
        }

        if (this.categoryId && this.elementId) {
            this.http.get<WorkflowElement>(this.baseUrl + 'api/Project/' + this.projectId + '/' + this.categoryId + '/' + this.elementId).subscribe(result => {
                this.element = result;
            }, error => console.error(error));
        }
    }
}
