import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { ProjectComponent } from './project/project.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { ProjectDetailsComponent } from './project-details/project-details.component';
import { ProjectSettingsComponent } from './project-settings/project-settings.component';
import { ProjectWrapperComponent } from './project-wrapper/project-wrapper.component';
import { ProjectSsllabsComponent } from './project-ssllabs/project-ssllabs.component';
import { ProjectKaliLinuxComponent } from './project-kali-linux/project-kali-linux.component';

import { AuthInterceptor } from './services/auth.interceptor';
import { ErrorInterceptor } from './services/error.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { AuthGuard } from './services/auth.guard';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        ProfileComponent,
        ProjectComponent,
        CreateProjectComponent,
        ProjectDetailsComponent,
        ProjectSettingsComponent,
        ProjectWrapperComponent,
        ProjectSsllabsComponent,
        ProjectKaliLinuxComponent,
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'create-project', component: CreateProjectComponent },
            { path: 'project/:id', component: ProjectWrapperComponent, canActivate: [AuthGuard]  },
            { path: 'project/:id/:category', component: ProjectWrapperComponent, canActivate: [AuthGuard]  },
            { path: 'project/:id/:category/:element', component: ProjectWrapperComponent, canActivate: [AuthGuard]  },
            { path: 'project', component: ProjectComponent, canActivate: [AuthGuard]  },
            { path: 'project-settings/:id', component: ProjectWrapperComponent, data: { 'settings': true }, canActivate: [AuthGuard]  },
            { path: 'project-ssllabs/:id', component: ProjectWrapperComponent, data: { 'ssllabs': true }, canActivate: [AuthGuard]  },
            { path: 'project-tooling/:id', component: ProjectWrapperComponent, data: { 'tooling': true }, canActivate: [AuthGuard]  },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
        ], { onSameUrlNavigation: 'reload' })
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true,
        },
        CookieService,
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
