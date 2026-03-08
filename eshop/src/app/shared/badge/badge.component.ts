import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-badge',
  template: `
    <ng-content></ng-content>
  `,
  styles: []
})
export class BadgeComponent implements OnInit {
  ngOnInit(): void {
  }

}
