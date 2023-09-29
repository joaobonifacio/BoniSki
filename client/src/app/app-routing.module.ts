import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  //esta é para https://localhost:4200/
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home' } },

  {path : 'shop', loadChildren: () => import('./shop/shop.module')
    .then(m=>m.ShopModule)},

 {
    path : 'basket', 
    //canActivate: [AuthGuard],
    loadChildren: () => import('./basket/basket.module')
      .then(m=>m.BasketModule)
  },

  {
    path : 'checkout', 
    canActivate: [AuthGuard],
    loadChildren: () => import('./checkout/checkout.module')
      .then(m=>m.CheckoutModule)
  },

  {path : 'account', loadChildren: () => import('./account/account.module')
    .then(m=>m.AccountModule)},

    {path : 'orders', loadChildren: () => import('./orders/orders.module')
    .then(m=>m.OrdersModule), data: { breadcrumb: 'Orders' }},

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
