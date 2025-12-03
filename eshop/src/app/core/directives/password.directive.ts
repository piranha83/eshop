import { Directive } from '@angular/core';
import { FormGroup, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms';
import { PasswordValidator } from '../validators/password.validator';

@Directive({
  selector: '[password]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: PasswordDirective,
      multi: true
    }
  ]
})
export class PasswordDirective implements Validator {
  validate(form: FormGroup): ValidationErrors | null {
    return PasswordValidator(form);
  }
}

