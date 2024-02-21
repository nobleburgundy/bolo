import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-past-games',
  templateUrl: './past-games.component.html',
  styleUrls: ['./past-games.component.css'],
})
export class PastGamesComponent {
  title = 'Past Games';
  public games: Game[] = [];

  constructor(http: HttpClient) {
    console.log('past games ctor environment.apiUrl', environment.apiUrl);

    http.get<Game[]>(environment.apiUrl + '/games').subscribe(
      (result) => {
        console.log('results', result);

        this.games = result;
      },
      (error) => console.error(error)
    );
  }
}

interface Game {
  winner: string;
  players: string[];
  date: Date;
}
