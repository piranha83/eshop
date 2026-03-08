import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
//ru
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import localeRuExtra from '@angular/common/locales/extra/ru';
import { AppRoutingModule } from './app-routing.module';
import { DataService, HttpDataService } from 'src/app/features/service/data.service';
import { PriceService } from 'src/app/features/service/price.service';
import { CartService } from 'src/app/features/service/cart.service';
import { CatalogService } from 'src/app/features/service/catalog.service';
import { RegisterModule } from './features/registeration/register.module';
import { PopupDirective } from './core/directives/popup.directive';
import { DeliverypageComponent } from './pages/deliverypage/deliverypage.component';
import { CacheDirective } from './core/directives/cache.directive';
import { FilterPipe } from './core/pipes/filter.pipe';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from './shared/shared.module';
import { OrderModule } from './features/order/order.module';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { environment } from 'src/environments/environment';
import Data from 'products.json';

registerLocaleData(localeRu, 'ru-RU', localeRuExtra);

@NgModule({
  declarations: [
    AppComponent, 
    PopupDirective, 
    DeliverypageComponent,
    CacheDirective,
    FilterPipe,
    ContactsComponent
  ],
  imports: [
    RegisterModule,
    OrderModule,
    SharedModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
  ],
  providers: [
    {provide: LOCALE_ID, useValue: 'ru-RU'},
    {
      provide: DataService,
      useFactory: function (http: HttpClient) {
        switch(environment.production)
        {
          case true:
            let ds = new HttpDataService(http);
            return ds;

          case false:
            let hds = new DataService();
            hds.setData(Data.products);
            return hds;
        }
      },
      deps: [HttpClient]
    },
    CartService,
    PriceService,
    CatalogService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
