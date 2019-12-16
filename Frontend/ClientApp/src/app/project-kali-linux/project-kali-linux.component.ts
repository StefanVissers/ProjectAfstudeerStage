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
    public loading = false;
    public error = false;


    constructor(private formbuilder: FormBuilder, private http: HttpClient,
        route: ActivatedRoute, @Inject('BASE_URL') private baseUrl: string) {
        route.params.subscribe(event => {
            this.projectId = event.id;
        });
    }

    ngOnInit() {
        this.toolingForm = this.formbuilder.group({
            projectId: [this.projectId],
            host: [''],
            ip: [''],
            nmapStandard: [false],
            niktoStandard: [false],
            xsserStandard: [false],
        });

        this.resultForm = this.formbuilder.group({
            nmapResult: [''],
            niktoResult: [''],
            xsserResult: [''],
        });
    }

    onFormSubmit() {
        this.error = false;
        this.loading = true;
        console.log(this.toolingForm.value);
        this.http.post<any>(this.baseUrl + 'api/Project/KaliLinuxTool/' + this.projectId, this.toolingForm.value).subscribe(result => {
            console.log(result);
            this.result = result;
            console.log(this.result);
            this.resultForm.patchValue(this.result);
            this.loading = false;
        }, error => {
            console.error(error);
            this.error = true;
            this.loading = false;
        })
    }



}
