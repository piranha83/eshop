import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconModule } from '../icon/icon.module';
import { IcontooltipComponent } from './icontooltip.component';
import { TooltipModule } from '../tooltip/tooltip.module';



@NgModule({
  declarations: [IcontooltipComponent],
  exports: [IcontooltipComponent],
  imports: [
    CommonModule,
    IconModule,
    TooltipModule
  ]
})
export class IcontooltipModule { }
