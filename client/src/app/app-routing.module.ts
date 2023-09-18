import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { BreadcrumbModule } from 'xng-breadcrumb';

const routes: Routes = [
  //esta é para https://localhost:4200/
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home' } },

  {path : 'shop', loadChildren: () => import('./shop/shop.module')
    .then(m=>m.ShopModule)},

    {path : 'basket', loadChildren: () => import('./basket/basket.module')
    .then(m=>m.BasketModule)},

    {path : 'checkout', loadChildren: () => import('./checkout/checkout.module')
    .then(m=>m.CheckoutModule)},

  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },

    //Esta é para routes que não existem
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    BreadcrumbModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
