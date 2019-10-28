import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project, WorkflowElementCategory, WorkflowElement } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

@Component({
    selector: 'app-project-details',
    templateUrl: './project-details.component.html',
    styleUrls: ['./project-details.component.css']
})
export class ProjectDetailsComponent implements OnInit {

    private projectId: string;
    @Input() project: Project;
    @Input() category: WorkflowElementCategory;
    @Input() element: WorkflowElement;
    public projectForm: FormGroup;
    public categoryId: string;
    public elementId: string;
    public isCollapsed = false;

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string,
        private activeRoute: ActivatedRoute) {
        route.params.subscribe(event => {
            this.projectId = event.id;
            this.categoryId = event.category;
            this.elementId = event.element;
        });

        this.projectForm = this.formbuilder.group({
            elementId: [''],
            explanation: [''],
            isDone: [''],
            isRelevant: [''],
        });
    }

    ngOnInit() {
        this.activeRoute.params.subscribe(_ => {
            this.loadElement();
        });
    }

    loadElement() {
        if (this.categoryId && this.elementId) {
            this.http.get<WorkflowElement>(this.baseUrl + 'api/Project/' + this.projectId + '/' + this.categoryId + '/' + this.elementId).subscribe(result => {
                this.element = result;
                this.projectForm.patchValue(this.element);
            }, error => console.error(error));
        }
    }

    onFormSubmit() {
        this.element = this.projectForm.value;
        console.log(JSON.stringify(this.element));
        this.http.put<Project>(this.baseUrl + 'api/Project/' + this.projectId + '/' + this.categoryId, this.element).subscribe(result => {
            this.project = result;
            this.loadElement();
        }, error => console.error(error));
    }
}
