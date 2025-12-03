import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-page404',
  template: `<div class="error"><h3>404 ошибка</h3><p>Ресрус не найден</p></div>`,
  styles: []
})
export class Page404Component implements OnInit {

  ngOnInit(): void {
  }

}
