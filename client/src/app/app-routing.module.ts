import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  //esta é para https://localhost:4200/
  { path: '', component: HomeComponent },

  {path : 'shop', loadChildren: () => import('./shop/shop.module')
    .then(m=>m.ShopModule)},
  
    //Esta é para routes que não existem
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
