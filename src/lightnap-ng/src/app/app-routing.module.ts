import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { adminGuard } from '@core/guards/admin.guard';
import { authGuard } from '@core/guards/auth.guard';

@NgModule({
    imports: [
        RouterModule.forRoot([
            { path: "", loadChildren: () => import("./public/components/pages/routes").then(m => m.ROUTES) },
            { path: "",
                component: AppLayoutComponent,
                canActivate: [authGuard],
                children: [
                    { path: "home", data: { breadcrumb: "Home" }, loadChildren: () => import("./secure/components/pages/routes").then(m => m.ROUTES) },
                    { path: "profile", data: { breadcrumb: "Profile" }, loadChildren: () => import("./profile/components/pages/routes").then(m => m.ROUTES) },
                ]
            },
            {
                path: "admin",
                component: AppLayoutComponent,
                canActivate: [authGuard, adminGuard],
                children: [{ path: "", data: { breadcrumb: "Admin" }, loadChildren: () => import("./admin/components/pages/routes").then(m => m.ROUTES) }],
            },
            { path: "identity", data: { breadcrumb: "Identity" }, loadChildren: () => import("./core/components/identity/pages/routes").then(m => m.ROUTES) },
            { path: "**", redirectTo: "/notfound" }
    ], { bindToComponentInputs: true, scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
