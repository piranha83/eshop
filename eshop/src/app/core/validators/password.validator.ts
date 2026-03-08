import { FormGroup, ValidationErrors } from "@angular/forms";

export function PasswordValidator(form: FormGroup): ValidationErrors | null {
    console.log('PasswordValidator');
    const pass = form.value || '';
    let count = 0;
    if( 8 <= pass.length  )
    {
        if(/.*\d.*/.test(pass))
            count ++;
        if(/.*[a-zA-Zа-яА-ЯЁё].*/.test(pass))
            count ++;
        if(/.*[\*\.\!\@\#\$\%\^\&\(\)\{\}\[\]\:\;\'\<\>\,\.\?\~\`\_\+\-\=\|].*/.test(pass))
            count ++;
    }

   return count >= 3 ? null : { "password": true };
}