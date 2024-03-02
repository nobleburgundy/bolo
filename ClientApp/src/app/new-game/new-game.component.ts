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
export class NewGameComponent {
  players: string[] = [];
  playerTypeahead = new FormControl();
  winnerTypeahead = new FormControl();
  playerScore = new FormControl();
  today = new Date();
  gameDateControl = new FormControl();
  filteredPlayers: string[] = [];
  addedPlayers: PlayerGame[] = [];

  constructor(http: HttpClient) {
    this.gameDateControl.setValue(formatDate(this.today, 'yyyy-MM-dd', 'en'));

    http.get<Player[]>(environment.apiUrl + '/players').subscribe(
      (result) => {
        // console.log('players results', result);

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
      name: this.playerTypeahead.value,
      score: this.playerScore.value,
    };
    console.log('playerGame: ', playerGame);
    this.addedPlayers.push(playerGame);
  }

  updateInput(event: any) {
    // super basic typeahead - needs to be improved
    event.preventDefault();
    console.log('updateInput: ' + event.target.value);
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
  }
}

interface PlayerGame {
  name: string;
  score: number;
}
