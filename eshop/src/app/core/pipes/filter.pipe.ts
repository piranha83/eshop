import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe<T> implements PipeTransform {

  transform(items: T[], predicate: (value: T, index: number) => unknown): any {
    if (!items) {
        return items;
    }
    return items.filter(predicate);
  }

}
