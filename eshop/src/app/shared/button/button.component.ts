import { Component, ElementRef, Input, OnChanges } from '@angular/core';
import { BaseComponent } from 'src/app/features/toggle/toggle.type';
import { ButtonColor, ButtonSize } from './button.type';

@Component({
  selector: 'app-button',
  template: `
    <button class="card card-small" type="button"
    [attr.disabled]="(this.isDisabled ? '' : null)"
    [ngClass]="class"
    [ngStyle]="style"
    (click)="onClick()" (mouseout)='onMouseOut()'><ng-content></ng-content></button>
  `,
  styles: []
})

export class ButtonComponent extends BaseComponent implements OnChanges {
  constructor(protected override host: ElementRef) { 
    super(host);
  }
  
  // active
  @Input() isActive: boolean = false;

  // disabled
  @Input() isDisabled: boolean = false;

  // color
  private _color: ButtonColor = 'default';

  @Input() set color(value: ButtonColor) {
    this._color = value;
  }

  // size
  private _size: ButtonSize = 'default';

  @Input() set size(value: ButtonSize) {
    this._size = value;
  }

  @Input() data: any;

  //

  get style() {
    return {
      'font-size': this._size,
      'line-height.px': this._size == 'small' ? 15 : 18
    };
  }

  get class() {
    return { 
      primary: this._color == 'primary', 
      accent: this._color == 'accent', 
      success: this._color == 'success',
      warning: this._color == 'warning',
      active: this.isActive
    };
  }

  //

  ngOnChanges(changes: any) {
    if (changes.color as ButtonColor) {
      console.log(changes.color);
    }
  }

  onClick() : void {
    this.isActive = true;
  }

  onMouseOut() : void {
    this.isActive = false;
  }
}
