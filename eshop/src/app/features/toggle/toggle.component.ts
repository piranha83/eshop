import { AfterContentInit, Component, ContentChildren, EventEmitter, Input, OnInit, Output, QueryList } from '@angular/core';
import {  mapTo, merge, Observable, Subject, takeUntil, tap } from 'rxjs';
import { ButtonComponent } from 'src/app/shared/button/button.component';
import { SortValueType, BaseComponent } from './toggle.type';

@Component({
  selector: 'app-toggle',
  template: `
  <div class="app-toggle">
    <ng-content select=".toggle"></ng-content>
    Сортировать: 
    <select class="button" (change)="onSortChanged($event.target)" [value]="sort">
      <option value="Default">Лучшее соответствие</option>
      <option value="PriceAsc">Cначала дешевле</option>
      <option value="PriceDesc">Cначала дороже</option>
    </select>
  </div>
  `,
  styles: []
})
export class ToggleComponent implements OnInit, AfterContentInit {
  @Input() sort: SortValueType = 'Default';
  @Output() changed = new EventEmitter<string>();
  @Output() sortChanged = new EventEmitter<SortValueType>();
  destroy$: Subject<boolean> = new Subject<boolean>();

  @ContentChildren(ButtonComponent)
  toggles!: QueryList<BaseComponent>

  constructor() {
  }

  ngAfterContentInit(): void {
    const clicks$: Observable<BaseComponent>[] = this.toggles.map(toggleItem => toggleItem.click$.pipe(mapTo(toggleItem)));
    merge(...clicks$).pipe(
      tap(toggleItem  => {
        this.toggles.map(toggleItem => toggleItem.isDisabled = false);
        toggleItem.isDisabled = true;
        this.onChanged(toggleItem.data);
      })
    )
    .pipe(takeUntil(this.destroy$))
    .subscribe();
  }

  ngOnInit(): void {
  }

  onChanged(value: string): void {
    this.changed.emit(value);
  }

  onSortChanged(e: any): void {
    const sortBy = e.value as SortValueType;
    this.sortChanged.emit(sortBy);
  }
}
