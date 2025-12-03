import { AfterViewInit, Directive, ElementRef, Input, Renderer2 } from '@angular/core';

export type Position = 'left' | 'right' | 'above' | 'below';

@Directive({
  selector: '[popup]'
})
export class PopupDirective implements AfterViewInit {
  menu!: any[];
  hidden: boolean = true;

  constructor(private renderer: Renderer2, private el: ElementRef) {
    console.log('popup');
  }

  @Input() position!: Position;

  @Input() set popup(menu: any[]) {
    this.menu = menu;
  }

  ngAfterViewInit() {
    const menuEl = this.renderer.createElement('ul');
    this.renderer.addClass(menuEl, 'submenu');
    this.renderer.addClass(menuEl, 'hidden');
    
    this.menu.forEach(item => {
      const itemEl = this.renderer.createElement('li');
      this.renderer.appendChild(itemEl, this.renderer.createText(item.title));
      this.renderer.appendChild(menuEl, itemEl);
    });
    this.renderer.appendChild(this.el.nativeElement, menuEl);
    this.renderer.listen(this.el.nativeElement, 'click', () => {
      this.hidden = !this.hidden;
      if(this.hidden)
        this.renderer.addClass(menuEl, 'hidden');
      else 
        this.renderer.removeClass(menuEl, 'hidden');
        this.stylepos = menuEl;
    });
    this.stylepos = menuEl;
  }

  set stylepos(menuEl: any) {
    if(document.documentElement.clientHeight < menuEl.offsetTop + menuEl.offsetHeight) {
      this.position = 'above';
    }
    switch(this.position) {
      case 'right': 
        this.renderer.setStyle(menuEl, 'margin-left', -this.el.nativeElement.offsetWidth + 'px');
        break;
      case 'above': 
        this.renderer.setStyle(menuEl, 'margin-top', -menuEl.offsetHeight + 'px');
        break;
    }
  }
}
