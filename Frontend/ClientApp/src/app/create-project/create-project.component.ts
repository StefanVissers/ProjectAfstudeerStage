import { Component, OnInit, Inject } from '@angular/core';
import { Project, WorkflowElementCategory, WorkflowElement, ASVSLevel, EnumEx } from 'src/app/models/project';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
    data = false;
    projectForm: FormGroup;
    massage: string;
    keys = Object.keys;
    asvsLevels: any;

    constructor(private formbuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        this.asvsLevels = this.getAsvsLevels();
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
        this.http.post<any>(this.baseUrl + 'api/Project', project, httpOptions).subscribe(result => {
            this.massage = result.Id;
            console.log(result);
        }, error => console.error(error));
    }

    public getAsvsLevels() {
        let prodTypes: any[] = [];

        //Get name-value pairs from ProductTypeEnum
        let prodTypeEnumList = EnumEx.getNamesAndValues(ASVSLevel);

        //Convert name-value pairs to ProductType[]
        prodTypeEnumList.forEach(pair => {
            let prodType = { 'id': pair.value.toString(), 'name': pair.name };
            prodTypes.push(prodType);
        });

        return prodTypes;
    }
}
