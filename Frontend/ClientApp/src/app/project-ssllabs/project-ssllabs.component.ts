import { Component, OnInit, Inject } from '@angular/core';
import { Project } from '../models/project';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-project-ssllabs',
    templateUrl: './project-ssllabs.component.html',
    styleUrls: ['./project-ssllabs.component.css']
})
export class ProjectSsllabsComponent implements OnInit {

    public project: Project;
    public ssllabsForm: FormGroup;
    public resultForm: FormGroup;
    private projectId: string;
    private result: any;
    private loading: boolean

    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });
    }

    ngOnInit() {
        this.ssllabsForm = this.formbuilder.group({
            projectId: [''],
            host: [''],
            ip: [''],
        });

        this.resultForm = this.formbuilder.group({
            grade: [''],
            serverName: [''],
            suites: [''],
            protocols: [''],
            sims: [''],
        })
    }

    // Load Everything from Database.
    loadSettings() {
        // Load project
        this.http.get<Project>(this.baseUrl + 'api/Project/' + this.projectId).subscribe(result => {
            this.project = result;
            this.ssllabsForm.patchValue(this.project);
        }, error => console.error(error));
    }

    onFormSubmit() {
        this.loading = true;

        let url = this.baseUrl + 'api/Project/SSLLabs/' + this.projectId + '/' + this.ssllabsForm.value.host + '/' + this.ssllabsForm.value.ip;
        console.log(url);
        this.http.get<any>(url).subscribe(result => {
            this.result = result;
            this.updateValues();
            console.log(result);
            this.loading = false;
        }, error => {
            console.error(error);
            this.loading = false;
        });
    }

    updateValues() {
        let suitesString = '';
        let protocolString = '';
        let simString = '';
        let object = {
            'grade': this.result.grade,
            'serverName': this.result.serverName,
            'suites': '',
            'protocols': '',
            'sims': '',
        }

        this.result.details.suites.forEach(function (value) {
            value.list.forEach(function (val) {
                suitesString = suitesString + val.name + '\n';
            });
        });

        this.result.details.protocols.forEach(function (value) {
            protocolString = protocolString + value.name + ' ' + value.version + '\n';
        });

        this.result.details.sims.results.forEach(function (value) {
            simString = simString + value.client.name + ' ' + value.client.version + ' ' + value.client.isReference + '\n';
        });

        object.suites = suitesString;
        object.protocols = protocolString;
        object.sims = simString;

        this.resultForm.patchValue(object);
    }
}
