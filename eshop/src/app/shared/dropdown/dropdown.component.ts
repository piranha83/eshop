import { Component, Input, OnInit } from '@angular/core';
import { DripDownTrigger } from './dropdown.type';

@Component({
  selector: 'app-dropdown',
  template: `
  <div class="dropdown">
    <app-button color="primary" (mouseover)='onToggleHover(true)' (mouseout)="onToggleHover(false)" (click)="onToggleClick()"><ng-content></ng-content></app-button>
    <app-menu [ngStyle]="this.display" [menuItems]="menuItems"></app-menu>
  </div>
  `,
  styles: []
})
export class DropdownComponent implements OnInit {
  @Input() trigger: DripDownTrigger = 'click';
  @Input() items: string = '';

  private _isOpen = false;
  
  get menuItems(): any { 
    return [
      'Dropdown option one', 
      'Dropdown option two',
      'Dropdown option tree'
    ];
  };

  get display() {
    return { 'display': this._isOpen ? '' : 'none' };
  }

  ngOnInit(): void {
  }

  onToggleClick() {
    if(this.trigger == 'click') {
      this._isOpen = !this._isOpen;
    }
  }

  onToggleHover(mouseOver: boolean) {
    if(this.trigger == 'hover') {
      this._isOpen = mouseOver;
    }
  }
}
