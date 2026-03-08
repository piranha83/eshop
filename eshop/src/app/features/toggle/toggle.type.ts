import { ElementRef, Injectable } from "@angular/core";
import { fromEvent, Observable } from "rxjs";

export type ToggleValueType = 'ShowAll' | 'InStock' | 'HasDiscount'
export type SortValueType = 'Default' | 'PriceAsc' | 'PriceDesc';

export interface Filter {
  sort: SortValueType;
  term: string;
  toggle: ToggleValueType;
  page: number,
  size: number,
}

export interface Toggle {
    readonly value: string;
    readonly label: string;
};

export function parseToggleValueType(val: string): ToggleValueType
{
    switch((val || '').toLocaleLowerCase())
    {
      case 'instock': return 'InStock';
      case 'hasdiscount': return 'HasDiscount';
      default: return 'ShowAll';
    }
}

export function toggleValueTypeToString(val: ToggleValueType): 'actionPrice' | 'available' | 'none'
{
    switch((val || '').toLocaleLowerCase())
    {
      case 'instock': return 'available';
      case 'hasdiscount': return 'actionPrice';
      default: return 'none';
    }
}

@Injectable()
export abstract class BaseComponent {
  click$: Observable<Event> = fromEvent(this.host.nativeElement, 'click');

  abstract data: any;

  abstract isDisabled: boolean;

  constructor(protected host: ElementRef) { 
  }
}