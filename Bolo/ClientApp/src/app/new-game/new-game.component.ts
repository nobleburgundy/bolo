import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Player } from '../models/Player';
import { environment } from 'src/environments/environment';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-new-game',
  templateUrl: './new-game.component.html',
})
export class NewGameComponent implements OnInit {
  httpClient: HttpClient;
  players: string[] = [];
  playerTypeahead = new FormControl();
  winnerTypeahead = new FormControl();
  playerScore = new FormControl();
  today = new Date();
  gameDateControl = new FormControl();
  filteredPlayers: string[] = [];
  addedPlayers: PlayerGame[] = [];

  constructor(http: HttpClient) {
    this.httpClient = http;
    this.gameDateControl.setValue(formatDate(this.today, 'yyyy-MM-dd', 'en'));
  }

  ngOnInit(): void {
    this.httpClient.get<Player[]>(environment.apiUrl + '/players').subscribe(
      (result) => {
        console.log('players results', result);
        this.players = result.map((p) => `${p.firstName} ${p.lastName}`);
      },
      (error) => console.error(error)
    );
  }

  filterPlayers() {
    const searchStr = this.playerTypeahead.value.toLowerCase();
    this.filteredPlayers = this.players.filter((player) =>
      player.toLowerCase().includes(searchStr)
    );
  }

  addPlayer(event: any) {
    event.preventDefault(); // Prevent losing focus
    console.log('addPlayer: ' + this.playerTypeahead.value);
    var playerGame = {
      firstName: this.playerTypeahead.value.split(' ')[0],
      lastName: this.playerTypeahead.value.split(' ').slice(1).join(' '),
      score: this.playerScore.value,
    };
    console.log('playerGame: ', playerGame);
    this.addedPlayers.push(playerGame);
  }

  updateInput(event: any) {
    // super basic typeahead - needs to be improved
    event.preventDefault();
    console.log('updateInput: ' + event.target.value, this.players);
    var foundPlayer = this.players.filter(
      (player) => player.indexOf(event.target.value) > -1
    );
    if (foundPlayer.length == 1) {
      this.playerTypeahead.setValue(foundPlayer[0]);
    }
  }

  addNewGame(event: any) {
    event.preventDefault();
    console.log('game date: ', this.gameDateControl.value);
    console.log('players/scores: ', this.addedPlayers);
    if (this.addedPlayers.findIndex((p) => p.score === 10000) === -1) {
      alert('There must be at least one winner with a score of 10000.');
      return;
    }
    this.httpClient
      .post(environment.apiUrl + '/games', {
        gameDate: this.gameDateControl.value,
        playerScores: this.addedPlayers,
      })
      .subscribe(
        (result) => {
          console.log('games post result', result);
        },
        (error) => console.error(error)
      );
  }
}

interface PlayerGame {
  firstName: string;
  lastName: string;
  score: number;
}
