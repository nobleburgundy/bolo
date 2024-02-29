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
  title = 'Past Games';
  public games: Game[] = [];
  public players: Player[] = [];

  constructor(http: HttpClient) {
    console.log('past games ctor environment.apiUrl', environment.apiUrl);

    http.get<Game[]>(environment.apiUrl + '/games').subscribe(
      (result) => {
        this.games = result;
        console.log('games results', this.games);
      },
      (error) => console.error(error)
    );

    http.get<Player[]>(environment.apiUrl + '/players').subscribe(
      (result) => {
        // console.log('players results', result);

        this.players = result;
      },
      (error) => console.error(error)
    );
  }

  getGameWinner(game: Game) {
    var winner = this.players.find((p) => p.id === game.winner);
    console.log('test game date' + game.game_Date);
    if (winner) {
      return `${winner?.firstName} ${winner?.lastName}`;
    }
    return null;
  }

  getGamePlayers(game: Game): string {
    let gamePlayers: Player[] = [];
    for (let i = 0; i < 5; i++) {
      gamePlayers[i] = this.players[this.randomInt(0, this.players.length - 1)];
    }
    // console.log('gamePlayers', gamePlayers);

    return gamePlayers.map((p) => `${p.firstName} ${p.lastName}`).join(', ');
  }

  randomInt(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
