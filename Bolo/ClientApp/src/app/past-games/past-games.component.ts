import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GamePlayer, Player } from '../models/Player';
import { Game } from '../models/Game';

@Component({
  selector: 'app-past-games',
  templateUrl: './past-games.component.html',
  styleUrls: ['./past-games.component.css'],
})
export class PastGamesComponent implements OnInit {
  httpClient: HttpClient;
  title = 'Past Games';
  public games: Game[] = [];

  constructor(http: HttpClient) {
    this.httpClient = http;
  }

  ngOnInit(): void {
    this.httpClient.get<Game[]>(environment.apiUrl + '/games').subscribe(
      (result) => {
        this.games = result;
        this.games.forEach(async (game) => {
          game.players = await this.httpClient
            .get<GamePlayer[]>(
              environment.apiUrl + '/games/' + game.id + '/players'
            )
            .toPromise()
            .then((result) => {
              return result ?? [];
            });
        });

        console.log('games results', this.games);
      },
      (error) => console.error(error)
    );
  }

  getGameWinner(game: Game): string {
    var winner = game.players.find((p) => p.score === 10000);
    return winner ? winner.firstName + ' ' + winner.lastName : 'No Winner';
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

  getGamePlayerNames(game: Game): string {
    return game.players.map((p) => p.firstName + ' ' + p.lastName).join(', ');
  }

  randomInt(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
