import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CatalogpageComponent } from './catalogpage.component';
import { CatalogRoutingModule } from './catalogrouting.module';
import { CatalogComponent } from './catalog.component';
import { ProductcardModule } from '../productcard/productcard.module';
import { CatalogService } from 'src/app/features/service/catalog.service';
import { StoreModule } from '@ngrx/store';
import { catalogReducer } from '../catalog/store/reducers/catalog.reducer'
import { cartReducer } from '../cart/store/reducers/cart.reducer'
import { EffectsModule } from '@ngrx/effects';
import { CatalogEffects } from './store/effects/catalog.effect';
import { CartModule } from '../cart/cart.module';
import { ButtonModule } from 'src/app/shared/button/button.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { FavoriteModule } from 'src/app/shared/favorite/favorite.module';
import { ToggleModule } from '../toggle/toggle.module';

@NgModule({
  declarations: [
    CatalogComponent,
    CatalogpageComponent
  ],
  exports: [
    CatalogComponent,
    CatalogpageComponent,
  ],
  imports: [
    CommonModule,
    CatalogRoutingModule,
    CartModule,
    SharedModule,
    ToggleModule,
    ProductcardModule, 
    ButtonModule,
    FavoriteModule,
    StoreModule.forFeature('catalog', catalogReducer),
    StoreModule.forFeature('cart', cartReducer),
    EffectsModule.forFeature([CatalogEffects]),
  ],
  providers: [
    CatalogService,
  ]
})
export class CatalogModule { }
