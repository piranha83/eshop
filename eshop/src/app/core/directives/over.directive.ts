import { Directive, ElementRef, EventEmitter, HostListener, Inject, Input, OnInit, Output, ViewChild } from '@angular/core';

@Directive({
  selector: '[over]',
  exportAs: 'over'
})
export class OverDirective implements OnInit {
  @Output() clickOut = new EventEmitter<MouseEvent>(); 
  
  constructor(private readonly el: ElementRef) { 
    console.log('PopupDirective');
  }

  @HostListener('document:click', ['$event'])
	onLeave(event: any) {
    if (!this.el.nativeElement.contains(event.target)) {
		  this.clickOut.emit(event);
		}
  }

  ngOnInit() {
  }
}
