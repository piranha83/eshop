import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RaitingComponent } from './raiting.component';



@NgModule({
  declarations: [RaitingComponent],
  exports: [RaitingComponent],
  imports: [
    CommonModule
  ]
})
export class RaitingModule { }
