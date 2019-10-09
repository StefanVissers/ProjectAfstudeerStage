"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var home_component_1 = require("./home/home.component");
var new_test_comp_component_1 = require("./new-test-comp/new-test-comp.component");
var login_component_1 = require("./login/login.component");
var auth_guard_1 = require("./services/auth.guard");
var counter_component_1 = require("./counter/counter.component");
var fetch_data_component_1 = require("./fetch-data/fetch-data.component");
var dashboard_component_1 = require("./dashboard/dashboard.component");
var register_component_1 = require("./register/register.component");
var appRoutes = [
    { path: '', component: home_component_1.HomeComponent, pathMatch: 'full' },
    { path: 'new-test-comp', component: new_test_comp_component_1.NewTestCompComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: 'counter', component: counter_component_1.CounterComponent },
    { path: 'fetch-data', component: fetch_data_component_1.FetchDataComponent },
    { path: 'register', component: register_component_1.RegisterComponent },
    { path: 'dashboard', component: dashboard_component_1.DashboardComponent },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map