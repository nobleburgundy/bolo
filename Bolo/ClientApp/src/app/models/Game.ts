import { Player } from "./Player";

export interface Game {
  id: number;
  winner: number;
  players: Player[];
  game_Date: string;
}
