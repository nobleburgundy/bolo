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
  players: Player[] = [];
  playerTypeahead = new FormControl();
  winnerTypeahead = new FormControl();
  playerScore = new FormControl();
  today = new Date();
  gameDateControl = new FormControl();
  filteredPlayers: Player[] = [];
  addedPlayers: Player[] = [];

  constructor(http: HttpClient) {
    this.httpClient = http;
    this.gameDateControl.setValue(formatDate(this.today, 'yyyy-MM-dd', 'en'));
  }

  ngOnInit(): void {
    this.httpClient.get<Player[]>(environment.apiUrl + '/players').subscribe(
      (result) => {
        console.log('players results', result);
        this.players = result;
      },
      (error) => console.error(error)
    );
  }

  filterPlayers() {
    const searchStr = this.playerTypeahead.value.toLowerCase();
    var searchFirst = searchStr.split(' ')[0];
    var searchLast = searchStr.split(' ').slice(1).join(' ');
    this.filteredPlayers = this.players.filter(
      (p) =>
        p.firstName.toLowerCase().indexOf(searchFirst) > -1 &&
        p.lastName.toLowerCase().indexOf(searchLast) > -1
    );
  }

  addPlayer(event: any) {
    event.preventDefault(); // Prevent losing focus
    console.log('addPlayer: ' + this.playerTypeahead.value);
    var playerGame = {
      firstName: this.playerTypeahead.value.split(' ')[0],
      lastName: this.playerTypeahead.value.split(' ').slice(1).join(' '),
      score: parseInt(this.playerScore.value),
    };

    var foundPlayer = this.players.find(
      (p) =>
        p.firstName === playerGame.firstName &&
        p.lastName === playerGame.lastName
    );
    console.log('playerGame: ', playerGame, 'foundplayer', foundPlayer);
    if (!foundPlayer) {
      alert('Player not found');
      return;
    }
    foundPlayer.score = playerGame.score;
    this.addedPlayers.push(foundPlayer);
  }

  updateInput(event: any) {
    // super basic typeahead - needs to be improved
    event.preventDefault();
    console.log('updateInput: ' + event.target.value, this.players);
    var searchStr = event.target.value.replace(' ', '').toLowerCase();

    var foundPlayer = this.players.filter((p) => {
      var playerString = (p.firstName + p.lastName).toLowerCase();
      return playerString.indexOf(searchStr) > -1;
    });
    if (foundPlayer.length == 1) {
      this.playerTypeahead.setValue(
        foundPlayer[0].firstName + ' ' + foundPlayer[0].lastName
      );
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
