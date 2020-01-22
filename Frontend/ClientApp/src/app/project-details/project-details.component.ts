import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Project, WorkflowElement } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-project-details',
    templateUrl: './project-details.component.html',
    styleUrls: ['./project-details.component.css']
})
export class ProjectDetailsComponent implements OnInit {

    private projectId: string;
    public categoryId: string;
    public elementId: string;
    public projectForm: FormGroup;
    @Input() project: any;
    @Input() category: any;
    @Input() element: any;

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
            description: [''],
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
        this.http.put<Project>(this.baseUrl + 'api/Project/' + this.projectId + '/' + this.categoryId, this.element).subscribe(result => {
            this.project = result;
            this.loadElement();
        }, error => console.error(error));
    }
}
