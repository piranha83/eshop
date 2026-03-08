import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-deliverypage',
  template: `
  <ng-template ngFor let-field [cacheOf]="object | keyvalue">  
    <p>{{field.key}} = {{field.value}}</p>
  </ng-template>
  `,
  styles: [''],
})
export class DeliverypageComponent implements OnInit {
  object: any = {
    adress: "Lenin st. 1, 20, 32",
    created: new Date().getSeconds()
  };

  constructor() { }

  ngOnInit(): void {
    setTimeout(() => {
      this.object.created = new Date();
      this.object['newField'] = new Date().getSeconds();
    }, 1000);
  }
}
