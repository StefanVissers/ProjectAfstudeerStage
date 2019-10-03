import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-new-test-comp',
  templateUrl: './new-test-comp.component.html',
  styleUrls: ['./new-test-comp.component.css']
})
export class NewTestCompComponent {
    public testClass: TestClass;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        http.post<TestClass>(baseUrl + 'api/SampleData/Tests', '').subscribe(result => {
            this.testClass = result;
        }, error => console.error(error));
    }


}

class TestClass {
    someString: string;
}
