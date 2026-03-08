import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-catalogpage',
  providers: [],
  template: `<app-catalog></app-catalog>`,
  styles: []
})
export class CatalogpageComponent implements OnInit {
  product: any;

  constructor() {}

  ngOnInit(): void {
  }
}
