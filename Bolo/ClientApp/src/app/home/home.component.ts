import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Game } from '../models/Game';
import { Player } from '../models/Player';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  games!: Game[];
  players!: Player[];
  constructor(http: HttpClient) {
    http.get<Game[]>(environment.apiUrl + '/games').subscribe(
      (result) => {
        console.log('games results', result);

        this.games = result;
      },
      (error) => console.error(error)
    );

    http.get<Player[]>(environment.apiUrl + '/players').subscribe(
      (result) => {
        console.log('players results', result);

        this.players = result;
      },
      (error) => console.error(error)
    );
  }
}
