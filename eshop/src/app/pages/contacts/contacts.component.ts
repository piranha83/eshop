import { Component, AfterViewInit } from '@angular/core';
import { environment } from '../../../environments/environment';

declare var maplibregl: any;

@Component({
  selector: 'app-contactspage',
  template: `
  <div class="contact">
    <h3>Россия, 123456, г. Москва, Кремлевская набережная, д. 1, корп. 1</h3>
    <h3>пн-ср 10:00-20:00, чт 10:00-18:00, пт 10:00-16:00 сб 10:00-18:00, вс - выходной</h3>
    <div id="map" *ngIf="!this.hiddenMap" class="map"></div>
  </div>
  `,
  styles: [''],
})
export class ContactsComponent implements AfterViewInit {
  hiddenMap: boolean = environment.production;

  ngAfterViewInit(): void {
    if (this.hiddenMap) return;

    const map = new maplibregl.Map({
      container: 'map',
      style: 'http://localhost:8081/liberty.json',
      center: [37.62, 55.75],
      zoom: 15
    });

    map.on('load', () => {
        map.addSource('points-data', {
            'type': 'geojson',
            'data': {
                'type': 'FeatureCollection',
                'features': [
                    {
                        'geometry': {
                            'type': 'Point',
                            'coordinates': [37.62, 55.75],
                        },
                        'properties': {
                            'title': 'Ждем вас у нас!'
                        }
                    },
                ]
            }
        });

        map.addLayer({
            'id': 'points-layer',
            'type': 'symbol',
            'source': 'points-data',
            'layout': {
                'text-field': ['get', 'title'],
            },
            'paint': {
                'text-color': '#000000ff',
            }
        });
    });
  }
}
