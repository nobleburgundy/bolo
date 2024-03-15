export interface Player {
  id: number;
  firstName: string;
  lastName: string;
  score: number;
  games: any[];
  wins: number;
  losses: number;
  gamesPlayed: number;
  winPercentage: string;
  averageScore: number;
}

export interface GamePlayer {
  playerId: number;
  gameId: number;
  firstName: string;
  lastName: string;
  score: number;
}

interface PlayerGame {
  firstName: string;
  lastName: string;
  score?: number;
  id: number;
}
