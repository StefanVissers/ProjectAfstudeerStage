import { Component } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    isExpanded = false;
    loggedIn = false;

    constructor(private cookieService: CookieService) {
        let cookieValue = this.cookieService.get('Auth');
        if (cookieValue) {
            this.loggedIn = true;
        }
    }

    logout() {
        this.cookieService.delete('Auth');
        this.loggedIn = false;
    }

    collapse() {
        this.isExpanded = false;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }
}
