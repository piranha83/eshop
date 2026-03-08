import { KeyValuePipe } from '@angular/common';
import { Directive, DoCheck, Input, KeyValueChanges, KeyValueDiffer, KeyValueDiffers, TemplateRef, TrackByFunction, ViewContainerRef } from '@angular/core';
export type Item = { [key: string]: any; }

@Directive({
  selector: '[cacheOf]'
})
export class CacheDirective implements DoCheck {
  _chache!: Item;

  private differ!: KeyValueDiffer<string, any>
  
  constructor(
    private readonly viewContainer: ViewContainerRef,
    private readonly templateRef: TemplateRef<any>,
    private readonly diff: KeyValueDiffers
    ) {
      console.log('cacheOf');
      this.differ = this.diff.find({}).create();
  }

  @Input() set cacheOf(val: Item) {
    this._chache = val;
  }

  ngDoCheck() {
    const changes = this.differ.diff(this._chache);
    if(changes) {
      this.applyChanges(changes);
    }
  }

  private applyChanges(changes: KeyValueChanges<string, any>) {
    changes.forEachAddedItem((rec) => {
      console.log(rec);
      if(rec.currentValue.key == null) return;
      this.viewContainer.createEmbeddedView(this.templateRef, {
        $implicit: {
            key: rec.currentValue.key,
            value: rec.currentValue.value
        }
      });
    });
  }
}
