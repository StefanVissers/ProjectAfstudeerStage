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

        this.http.get<any>(this.baseUrl + 'api/Project/GetToolingReport/' + this.projectId).subscribe(res => {
            this.result = res;
            if (this.result) {
                this.updateValues();
            }
        })
    }

    ngOnInit() {
        this.toolingForm = this.formbuilder.group({
            projectId: [this.projectId],
            hostname: [''],
            ip: [''],
            nmapStandard: [false],
            niktoStandard: [false],
            xsserStandard: [false],
            nmapAdditionalArgs: [''],
            niktoAdditionalArgs: ['-host'],
            xsserAdditionalArgs: ['-u'],
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
        this.http.post<any>(this.baseUrl + 'api/Project/KaliLinuxTool/' + this.projectId, this.toolingForm.value).subscribe(result => {
            this.result = result;
            this.updateValues();
            this.loading = false;
        }, error => {
            console.error(error);
            this.error = true;
            this.loading = false;
        })
    }

    updateValues() {
        this.resultForm.patchValue(this.result);
    }

}
