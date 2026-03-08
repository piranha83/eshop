import { FormGroup, ValidationErrors } from "@angular/forms";

export function PasswordMatchValidator(name: string, confirmName: string, form: FormGroup): ValidationErrors | null {
    const passwordField = form.controls[name];
    const confirmField = form.controls[confirmName];

    if(!passwordField || ! confirmField) {
        return null;
    }

    if(confirmField.errors && !confirmField.errors["passwordMatch"]) {
        return null;
    }
   
    const invalid = passwordField.value != confirmField.value;
    invalid
        ? confirmField.setErrors({ "passwordMatch": true })
        : confirmField.setErrors(null);

    return invalid
        ? { "passwordMatch": true }
        : null;
}