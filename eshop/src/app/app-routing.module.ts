import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { Page404Component } from './pages/page404/page404.component';
import { RegisterComponent } from './features/registeration/register.component';
import { DeliverypageComponent } from './pages/deliverypage/deliverypage.component';
import { OrderComponent } from './features/order/order.component';
import { ContactsComponent } from './pages/contacts/contacts.component';

export const routes: Routes = [
  { path: 'catalog', loadChildren: () => import('./features/catalog/catalog.module').then(m => m.CatalogModule) },
  { path: '', redirectTo: 'catalog', pathMatch: 'full' },
  { path: '404', component: Page404Component },
  { path: 'register', component: RegisterComponent },
  { path: 'order', component: OrderComponent },
  { path: 'delivery', component: DeliverypageComponent },
  { path: 'contacts', component: ContactsComponent },
  { path: '**', redirectTo: '404', pathMatch: 'full' },
];

export const option: ExtraOptions = {
  enableTracing: false,
  scrollPositionRestoration: 'enabled'
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes, option) 
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
