import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register.component';
import { FormsModule } from '@angular/forms';
import { PasswordMatchDirective } from 'src/app/core/directives/password-match.directive';
import { PasswordDirective } from 'src/app/core/directives/password.directive';

@NgModule({
  exports: [RegisterComponent],
  declarations: [ 
    RegisterComponent,
    PasswordMatchDirective,
    PasswordDirective,
  ],
  imports: [
    FormsModule,
    CommonModule
  ]
})
export class RegisterModule { }
