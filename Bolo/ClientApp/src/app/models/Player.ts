export interface Player {
  id: number;
  firstName: string;
  lastName: string;
  score: number;
  games: any[];
  wins: number;
  losses: number;
  gamesPlayed: number;
  winPercentage: number;
  averageScore: number;
}

export interface GamePlayer {
  playerId: number;
  gameId: number;
  firstName: string;
  lastName: string;
  score: number;
}
