import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'eshop';
  items = [{title: 'Пункты выдачи', url: 'shops'}, {title: 'Постаматы', url: 'posts'}, {title: 'Доставка', url: 'delivery'}];
}
