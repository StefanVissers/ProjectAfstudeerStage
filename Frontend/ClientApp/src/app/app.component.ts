import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    public loggedIn = false;
    constructor(private cookieService: CookieService, private route: ActivatedRoute) {

    }

    ngOnInit(){
        this.route.params.subscribe(_ => {
            this.isLoggedIn();
        });
    }

    isLoggedIn() {
        let cookieValue = this.cookieService.get('Auth');
        if (cookieValue) {
            this.loggedIn = true;
        } else {
            this.loggedIn = false;
        }
    }

    title = 'app';
}
