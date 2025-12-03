import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsComponent } from './forms.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  exports: [FormsComponent],
  declarations: [
    FormsComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: []
})
export class FormModule { }
