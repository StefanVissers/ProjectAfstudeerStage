import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { NewTestCompComponent } from './new-test-comp/new-test-comp.component'
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './services/auth.guard';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RegisterComponent } from './register/register.component';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'new-test-comp', component: NewTestCompComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'dashboard', component: DashboardComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);
