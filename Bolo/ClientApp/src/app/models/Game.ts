import { GamePlayer, Player } from './Player';

export interface Game {
  id: number;
  winner: number;
  players: GamePlayer[];
  game_Date: string;
}
