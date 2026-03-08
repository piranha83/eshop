import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { parseToggleValueType } from '../toggle/toggle.type';

@Injectable()
export class ResolverGuard implements Resolve<any> {
  constructor() {}

  resolve(
    route: ActivatedRouteSnapshot,
    ): Observable<any> | Promise<any> | any {
      return parseToggleValueType(route.queryParams['toggle']);
  }
}
