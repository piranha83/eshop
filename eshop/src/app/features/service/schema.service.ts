import { Injectable } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { Order } from "src/app/features/order/order.type";

export interface Schema {
  form: FormGroup,
  options: { [key: string] : any }
}

@Injectable()
export class SchemaService {
  private options = { updateOn: 'blur' };

  constructor(readonly fb: FormBuilder) {}

  forOrder(order: Order | undefined): Schema {
    return {
      form: this.fb.group({
        name: [
          order?.name, 
          {
            validators: [Validators.required, Validators.pattern("^[a-zA-Zа-яА-ЯёЁ]+ [a-zA-Zа-яА-Я\sёЁ]+$")], 
            updateOn: 'blur'
          }
        ],
        phone: [
          order?.phone, 
          { 
            validators: [Validators.required, Validators.pattern("[\+]{0,1}[0-9]{11}")],
            updateOn: 'blur'
          }
        ],
        delivery: [
          order?.delivery,
          { 
            validators: [Validators.required],
            updateOn: 'blur'
          }],
        address: this.fb.group({
          street: [order?.address?.street, [Validators.required]],
          city:   [order?.address?.city, [Validators.required]],
          state:  [order?.address?.state, [Validators.required]],
          zip:    [order?.address?.zip, [Validators.required, Validators.pattern("[0-9]+")]]
        }, { updateOn: 'blur' }),
        checkout: [
          order?.checkout,
          { 
            validators: [Validators.required],
            updateOn: 'blur'
          }],
      }, { updateOn: 'blur' }),
      options: {
        name:     { label : 'Фамилия Имя', type: '' },
        phone:    { label : 'Телефон', type: '' },
        address:  { label : 'Адрес доставки', type: 'label' },
        street:   { label : 'Улица', type: '' },
        city:     { label : 'Город', type: '' },
        state:    { label : 'Штат', type: '' },
        zip:      { label : 'Индекс', type: '' },
        delivery: { label : 'Способ получения товара', type: 'label' },
        delivery1:   { label : 'доставка на дом', type: 'radio' },
        delivery2:   { label : 'забрать в магазине', type: 'radio' },
      }
    };
  }

  deliveryValidator(): ValidatorFn { 
    return (control: AbstractControl) : ValidationErrors | null => {
      const value = control.value;
      console.log(value);
      return null;
    }
  }
}
