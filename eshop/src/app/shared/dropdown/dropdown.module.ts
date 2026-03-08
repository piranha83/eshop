import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropdownComponent } from './dropdown.component';
import { ButtonModule } from '../button/button.module';
import { MenuModule } from '../menu/menu.module';



@NgModule({
  declarations: [DropdownComponent],
  exports: [DropdownComponent],
  imports: [
    CommonModule,
    ButtonModule,
    MenuModule
  ]
})
export class DropdownModule { }
