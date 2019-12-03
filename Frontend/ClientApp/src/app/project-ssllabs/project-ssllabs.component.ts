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
    private error: string;
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
            subject: [''],
            altNames: [''],
            validFrom: [''],
            validUntil: [''],
            key: [''],
            issuer: [''],
            sigAlg: [''],
            revocationStatus: [''],
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
        this.error = "";
        this.loading = true;
        let url = this.baseUrl + 'api/Project/SSLLabs/' + this.projectId;
        console.log(url);
        this.http.post<any>(url, { host: this.ssllabsForm.value.host, ip: this.ssllabsForm.value.ip }).subscribe(result => {
            console.log(result);
            if (result.status == "READY") {
                this.result = result;
                this.updateValues();
            } else {
                this.error = result.status;
            }
            
            this.loading = false;
        }, error => {
                console.error(error);
                this.error = "";
                this.loading = false;
        });
    }

    updateValues() {
        let ipv4Endpoint = this.result.endpoints[1];
        let altNames = '';
        let suitesString = '';
        let protocolString = '';
        let simString = '';
        let object = {
            'grade': ipv4Endpoint.grade,
            'serverName': ipv4Endpoint.serverName,
            'subject': this.result.certs[0].subject,
            'validFrom': this.result.certs[0].notBefore,
            'validUntil': this.result.certs[0].notAfter,
            'issuer': this.result.certs[0].issuerSubject,
            'key': this.result.certs[0].keyAlg + " " + this.result.certs[0].keySize,
            'sigAlg': this.result.certs[0].sigAlg,
            'revocationStatus': this.result.certs[0].revocationStatus,
            'altNames': '',
            'suites': '',
            'protocols': '',
            'sims': '',
        }

        this.result.certs[0].altNames.forEach(function (value) {
            altNames += value + "\n";
        })

        ipv4Endpoint.details.suites.forEach(function (value) {
            value.list.forEach(function (val) {
                suitesString = suitesString + val.name + '\n';
            });
        });

        ipv4Endpoint.details.protocols.forEach(function (value) {
            protocolString = protocolString + value.name + ' ' + value.version + '\n';
        });

        ipv4Endpoint.details.sims.results.forEach(function (value) {
            simString = simString + value.client.name + ' ' + value.client.version + ' ' + value.errorCode + '\n';
        });

        object.suites = suitesString;
        object.protocols = protocolString;
        object.sims = simString;
        object.altNames = altNames;

        this.resultForm.patchValue(object);
    }
}
