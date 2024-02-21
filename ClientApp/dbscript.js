const { Client } = require("pg");
const _ = require("lodash");
const { randomInt } = require("crypto");

async function main() {
  const client = new Client({
    user: "jgould",
    host: "localhost",
    database: "bolo_db",
    password: "db123",
    port: 5432,
  });

  await client.connect();

  await client.query(`
        CREATE TABLE IF NOT EXISTS Players (
            Id SERIAL PRIMARY KEY,
            FirstName VARCHAR(255),
            LastName VARCHAR(255)
        )
    `);

  await client.query(`
        CREATE TABLE IF NOT EXISTS Games (
            Id SERIAL PRIMARY KEY,
            Winner INTEGER REFERENCES Players(Id),
            Score INTEGER
        )
    `);

  await client.query(`
        CREATE TABLE IF NOT EXISTS GamePlayers (
            GameId INTEGER REFERENCES Games(Id),
            PlayerId INTEGER REFERENCES Players(Id)
        )
    `);

  const players = _.range(10).map(() => ({
    FirstName: "TestFirst" + randomInt(10),
    LastName: "TestLast" + randomInt(1000),
  }));

  for (const player of players) {
    await client.query(
      "INSERT INTO Players (FirstName, LastName) VALUES ($1, $2)",
      [player.FirstName, player.LastName]
    );
  }

  const { rows } = await client.query("SELECT Id FROM Players");
  const playerIds = rows.map((row) => row.id);

  const games = _.range(100).map(() => ({
    Winner: _.sample(playerIds),
    Score: 10000,
  }));

  for (const game of games) {
    const res = await client.query(
      "INSERT INTO Games (Winner, Score) VALUES ($1, $2) RETURNING Id",
      [game.Winner, game.Score]
    );
    const gameId = res.rows[0].id;
    const gamePlayers = _.sampleSize(playerIds, _.random(4, 6));
    for (const playerId of gamePlayers) {
      await client.query(
        "INSERT INTO GamePlayers (GameId, PlayerId) VALUES ($1, $2)",
        [gameId, playerId]
      );
    }
  }

  await client.end();
}

main().catch(console.error);
console.log("DB test data script complete.");
