import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Player } from '../models/Player';
import { Game } from '../models/Game';

@Component({
  selector: 'app-past-games',
  templateUrl: './past-games.component.html',
  styleUrls: ['./past-games.component.css'],
})
export class PastGamesComponent {
  httpClient: HttpClient;
  title = 'Past Games';
  public games: Game[] = [];

  constructor(http: HttpClient) {
    console.log('past games ctor environment.apiUrl', environment.apiUrl);
    this.httpClient = http;

    this.httpClient.get<Game[]>(environment.apiUrl + '/games').subscribe(
      (result) => {
        this.games = result;
        this.games.forEach((game) => {
          game.players = this.getGamePlayers(game);
        });

        console.log('games results', this.games);
      },
      (error) => console.error(error)
    );
  }

  getGameWinner(game: Game) {
    console.log('test game date' + game.game_Date);
    // if (winner) {
    //   return `${winner?.firstName} ${winner?.lastName}`;
    // }
    return 'Townes Meyer Gould';
  }

  getGamePlayers(game: Game): Player[] {
    let gamePlayers: Player[] = [];

    this.httpClient
      .get<Player[]>(environment.apiUrl + '/games/' + game.id + '/players')
      .subscribe(
        (result) => {
          gamePlayers = result;
          console.log('game players results', gamePlayers);
        },
        (error) => console.error(error)
      );

    return gamePlayers;
  }

  randomInt(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
