import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, mergeMap } from "rxjs";
import { CatalogService } from "src/app/features/service/catalog.service";
import { enterCatalogAction, errorCatalogAction, loadedCatalogAction } from "../actions/catalog.action";

@Injectable()
export class CatalogEffects {

  loadProducts$ = createEffect(() => this.action$.pipe(
    ofType(enterCatalogAction),
    mergeMap(action => this.catalogService.find(action.filter).pipe(
        map(products => loadedCatalogAction({products}),
        catchError(error => of(errorCatalogAction({error}))))
    ))
  ));

  constructor(
    private readonly action$: Actions,
    private readonly catalogService: CatalogService) {
  }
}