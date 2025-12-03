import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Register } from './register.type';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: [''],
})
export class RegisterComponent implements OnInit {
  valid(form: NgForm, name: string): boolean {
    return form.controls[name]?.valid || form.controls[name]?.untouched
  }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm): void {
    if(!form.control.valid) {
      return;
    }
    console.log(form.control.value as Register);
  }

}
