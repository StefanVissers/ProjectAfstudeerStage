import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-project-kali-linux',
    templateUrl: './project-kali-linux.component.html',
    styleUrls: ['./project-kali-linux.component.css']
})
export class ProjectKaliLinuxComponent implements OnInit {
    public result: any;
    public toolingForm: FormGroup;
    public resultForm: FormGroup;
    private projectId: string;


    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });

        this.http.post<any>(this.baseUrl + 'api/Project/KaliLinuxTool/' + this.projectId, { command: "nmap -V" }).subscribe(result => {
            console.log(result);
            this.result = result.message;
            console.log(this.result);
        })
    }

    ngOnInit() {
        this.toolingForm = this.formbuilder.group({
            projectId: [''],
            host: [''],
            ip: [''],
            nmapStandard: [''],
            niktoStandard: [''],
            xsserStandard: [''],
        });

        this.resultForm = this.formbuilder.group({
            nmapResult: [''],
            niktoResult: [''],
            xsserResult: [''],
        });
    }

    onFormSubmit() {
        this.http.post<any>(this.baseUrl + 'api/Project/KaliLinuxTool/' + this.projectId, { command: "nmap -V" }).subscribe(result => {
            console.log(result);
            this.result = result.message;
            console.log(this.result);
        })
    }



}
