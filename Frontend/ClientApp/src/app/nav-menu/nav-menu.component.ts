import { Component, Input, OnInit, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, AfterViewChecked{
    isExpanded = false;
    show = false;
    @Input() loggedIn;

    constructor(private cdRef: ChangeDetectorRef, private authenticationService: AuthenticationService, private cookieService: CookieService, private route: ActivatedRoute) {
        this.isLoggedIn();
    }

    ngOnInit() {
        this.route.params.subscribe(_ => {
            this.isLoggedIn();
        });
    }

    ngAfterViewChecked() {
        let show = this.loggedIn;
        if (show != this.show) { // check if it change, tell ChangeDetector update view
            this.show = show;
            this.cdRef.detectChanges();
        }
    }

    logout() {
        this.authenticationService.logout();
        this.loggedIn = false;
    }

    collapse() {
        this.isExpanded = false;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }

    isLoggedIn() {
        let cookieValue = this.cookieService.get('Auth');
        if (cookieValue) {
            this.loggedIn = true;
            return this.loggedIn;
        } else {
            this.loggedIn = false;
            return this.loggedIn;
        }
    }
}
