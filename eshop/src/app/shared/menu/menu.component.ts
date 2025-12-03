import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-menu',
  template: `
  <ul> 
    <li *ngFor="let item of menuItems" (click)="onSelect(item)">{{item}}</li>
  </ul>
  `,
  styles: []
})
export class MenuComponent implements OnInit {
   // active
   @Input() menuItems: string[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  onSelect(item: string): void {
    console.log(item)
  }
}
