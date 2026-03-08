import { newArray } from '@angular/compiler/src/util';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { Schema } from 'src/app/features/service/schema.service';

@Component({
  selector: 'app-forms',
  templateUrl: './forms.component.html',
  styles: ['']
})
export class FormsComponent implements OnInit {
  @Input() schema : Schema = { form: new FormGroup({}), options: { } };
  @Input() data: any | undefined;
  @Output() submit = new EventEmitter<any>();

  invalid(control: AbstractControl): boolean {
    return control?.invalid && control?.untouched;
  }
  
  render(form: FormGroup, parent: string = ''): any[] {
    let controls: any[] = [];

    Object.keys(form.controls)
      .map(name => {
        const control = form.get(name);
        control?.markAsDirty();
        controls.push({ 
          name,
          control: control,
          label: this.schema?.options[name]?.label || name,
          type: this.schema?.options[name]?.type,
          parent: parent,
        });
        if(control instanceof FormGroup) { 
          controls = controls.concat(this.render(control, name));
        } 
      });

    return controls;
  }

  formControlName(name: string) {
    return this.schema?.form ? name : null;
  }

  ngOnInit(): void {
    console.log('forms');
    this.schema?.form.reset();
  }

  onSubmit(): void {
    console.log(this.schema.form.value);
    if(!this.schema?.form.invalid) {
      return;
    }
    this.submit.emit(this.schema.form.value);
  }
}
