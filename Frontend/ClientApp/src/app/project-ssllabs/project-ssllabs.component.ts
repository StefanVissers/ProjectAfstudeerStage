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
        console.log(this.projectId);
        this.http.get<any>(this.baseUrl + 'api/Project/GetSSLLabsReport/' + this.projectId).subscribe(res => {
            this.result = res;
            console.log(this.result);
            if (this.result) {
                this.updateValues();
            }
        })
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
            trusted: [''],
            forwardSecrecy: [''],
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

    // Long function for filling the resultform.
    updateValues() {
        let ipv4Endpoint = this.result.endpoints.length == 1 ? this.result.endpoints[0] : this.result.endpoints[1];
        let altNames = '';
        let suitesString = '';
        let protocolString = '';
        let simString = '';
        let trusted = '';
        let object = {
            'grade': ipv4Endpoint.grade,
            'serverName': ipv4Endpoint.serverName,
            'subject': '',
            'validFrom': '',
            'validUntil': '',
            'issuer': '',
            'key': '',
            'sigAlg': '',
            'revocationStatus': '',
            'altNames': '',
            'suites': '',
            'protocols': '',
            'sims': '',
            'trusted': '',
            'forwardSecrecy': '',
        }

        let subject = this.result.certs[0].subject;
        let validFrom = new Date(this.result.certs[0].notBefore).toString();
        let validUntil = new Date(this.result.certs[0].notAfter).toString();
        let issuer = this.result.certs[0].issuerSubject;
        let key = this.result.certs[0].keyAlg + " " + this.result.certs[0].keySize;
        let sigAlg = this.result.certs[0].sigAlg;
        let revocationStatus = (this.result.certs[0].revocationStatus == 1 ? 'Certificate Revoked' :
            this.result.certs[0].revocationStatus == 2 ? 'Certificate Not Revoked' : 'No information');
        let forwardSecrecy = (ipv4Endpoint.details.forwardSecrecy == 1 ? 'At least one browser negotiated Forward Secrecy' :
            ipv4Endpoint.details.forwardSecrecy == 2 ? 'Modern browsers used Forward Secrecy' :
                ipv4Endpoint.details.forwardSecrecy == 4 ? 'All simulated browsers achieved Forward Secrecy' : 'No browsers used Forward Secrecy')

        this.result.certs[0].altNames.forEach(function (value) {
            altNames += value + ', ';
        });

        ipv4Endpoint.details.suites.forEach(function (value) {
            value.list.forEach(function (val) {
                suitesString = suitesString + val.name + ' \n';
            })
        });

        ipv4Endpoint.details.protocols.forEach(function (value) {
            protocolString = protocolString + value.name + ' ' + value.version + ' \n';
        });

        ipv4Endpoint.details.sims.results.forEach(function (value) {
            simString = simString + value.client.name + ' ' + value.client.version + ' ' +
                (value.errorCode == 1 ? 'not supported' : 'supported') + ' ' + value.suiteName + ' \n';
        });

        ipv4Endpoint.details.certChains.forEach(function (value) {
            value.trustPaths.forEach(function (val) {
                trusted += val.trust[0].rootStore + ': ' + (val.trust[0].isTrusted ? 'Trusted' : 'Not Trusted') + ' \n'
            })
        });

        object.subject = subject;
        object.validFrom = validFrom;
        object.validUntil = validUntil;
        object.issuer = issuer;
        object.key = key;
        object.sigAlg = sigAlg;
        object.suites = suitesString;
        object.protocols = protocolString;
        object.sims = simString;
        object.altNames = altNames;
        object.revocationStatus = revocationStatus;
        object.forwardSecrecy = forwardSecrecy;
        object.trusted = trusted;

        this.resultForm.patchValue(object);
    }
}
