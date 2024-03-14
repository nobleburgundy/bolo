import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Game } from '../models/Game';
import { Player } from '../models/Player';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
/**
 * Represents the HomeComponent class.
 */
export class HomeComponent implements OnInit {
  games!: Game[];
  players!: Player[];
  httpClient: HttpClient;

  constructor(http: HttpClient) {
    this.httpClient = http;
  }

  /**
   * Initializes the component.
   */
  ngOnInit(): void {
    this.httpClient
      .get<Game[]>(environment.apiUrl + '/games')
      .pipe(
        switchMap((games) => {
          this.games = games;
          console.log('games', games);
          return this.httpClient.get<Player[]>(environment.apiUrl + '/players');
        })
      )
      .subscribe(
        (players) => {
          console.log('players results', players, 'games', this.games);
          this.players = players;
          var i = 0;
          this.players.forEach((player) => {
            this.players[i].games = this.games.filter((game) => {
              return game.players.some((p) => p.playerId === player.id);
            });

            var playerWins = 0;
            this.games.forEach((game: Game) => {
              var playerGame = game.players.find(
                (p) => p.playerId === player.id
              );
              if (playerGame && playerGame.score === 10000) {
                playerWins++;
              }
            });
            console.log('wins', playerWins);
            this.players[i].wins = playerWins;

            this.players[i].gamesPlayed = this.getGamesPlayed(player.id);

            this.players[i].losses =
              player.gamesPlayed > 0 ? player.gamesPlayed - player.wins : 0;

            this.players[i].winPercentage =
              player.gamesPlayed > 0 ? player.wins / player.gamesPlayed : 0;

            this.players[i].averageScore = this.getAverageScore(
              player.id,
              this.games
            );
            i++;
          });
        },
        (error) => console.error(error)
      );
  }

  /**
   * Calculates the average score for a player.
   * @param id - The player ID.
   * @param games - The list of games.
   * @returns The average score for the player.
   */
  getAverageScore(id: number, games: Game[]): number {
    var playerGames = games.filter((game) =>
      game.players.some((p) => p.playerId === id)
    );
    var totalScore = 0;
    playerGames.forEach((game) => {
      var playerGame = game.players.find((p) => p.playerId === id);
      if (playerGame) {
        totalScore += playerGame.score;
      }
    });
    return totalScore / (playerGames.length > 0 ? playerGames.length : 1);
  }

  /**
   * Gets the number of games played by a player.
   * @param playerId - The player ID.
   * @returns The number of games played by the player.
   */
  getGamesPlayed(playerId: number): number {
    return this.games.filter((game) =>
      game.players.some((p) => p.playerId === playerId)
    ).length;
  }
}

interface GamePlayer {
  playerId: number;
  score: number;
  firstName: string;
  lastName: string;
}
