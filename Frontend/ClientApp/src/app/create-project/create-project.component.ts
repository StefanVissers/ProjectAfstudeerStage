import { Component, OnInit, Inject } from '@angular/core';
import { Project } from 'src/app/models/project';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
    data = false;
    projectForm: FormGroup;
    keys = Object.keys;
    asvsLevels: any;

    constructor(private router: Router, private formbuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    ngOnInit() {
        this.projectForm = this.formbuilder.group({
            Name: ['', [Validators.required]],
            Description: ['', [Validators.required]],
            ASVSLevel: ['', [Validators.required]],
        });
    }

    onFormSubmit() {
        const project = this.projectForm.value;
        this.CreateProject(project);
    }

    CreateProject(project: Project) {
        const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
        this.http.post<Project>(this.baseUrl + 'api/Project', project, httpOptions).subscribe(result => {
            this.router.navigate(['/project/' + result.id]);
        }, error => console.error(error));
    }
}
