import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-past-games',
  templateUrl: './past-games.component.html',
  styleUrls: ['./past-games.component.css'],
})
export class PastGamesComponent {
  title = 'Past Games';
  public games: Game[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log('past games ctor');

    http.get<Game[]>(baseUrl + 'api/games').subscribe(
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
