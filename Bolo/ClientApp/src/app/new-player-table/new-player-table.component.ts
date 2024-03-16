import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Player } from '../models/Player';

@Component({
  selector: 'app-new-player-table',
  templateUrl: './new-player-table.component.html',
  styleUrls: ['./new-player-table.component.css'],
})
export class NewPlayerTableComponent {
  @Input() addedPlayers: Player[] = [];
  @Output() addedPlayersChange = new EventEmitter<any>();

  removePlayer(playerId: number) {
    this.addedPlayers = this.addedPlayers.filter((p) => p.id !== playerId);
    this.addedPlayersChange.emit(this.addedPlayers);
  }

  showTieBreakColumn(): boolean {
    return this.addedPlayers.filter((p) => p.score === 10000).length > 1;
  }
}
