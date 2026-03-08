import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-raiting',
  template: `
  <div>
    <span class="material-icons app-raiting" [ngClass]="rating(i)" *ngFor="let i of [0, 1, 2, 3, 4]"  (click)="onRateClick(i)">
      star_rate
    </span>
  </div>`,
  styles: []
})
export class RaitingComponent implements OnInit {
  @Input() rate?: number | undefined;
  @Output() Rate = new EventEmitter<number>();

  ngOnInit(): void {
  }

  rating(i: number) {
    return i <= (this.rate || 0) ? 'active' : ''; 
  }

  onRateClick(rate: number) {
    this.Rate.emit(rate);
  }

}
