import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project, WorkflowElementCategory, WorkflowElement } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

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

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string,
        private activeRoute: ActivatedRoute) {
        route.params.subscribe(event => {
            this.projectId = event.id;
            this.categoryId = event.category;
            this.elementId = event.element;
        });
        this.activeRoute.data.subscribe(data => {
            this.settings = data.settings;
        })
    }

    ngOnInit() {
        this.activeRoute.params.subscribe(_ => {
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

    getActiveAccordion() {
        if (this.categoryId) {
            return this.categoryId;
        }
    }
}
