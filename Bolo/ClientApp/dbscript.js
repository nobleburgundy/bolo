// user: "jgould",
// host: "localhost",
// database: "bolo_db",
// password: "db123",
// port: 5432,

// dbscript.js
const { Sequelize, DataTypes } = require("sequelize");
const sequelize = new Sequelize(
  "postgres://jgould:db123@localhost:5432/bolo_db"
); // replace with your connection string

const Player = sequelize.define(
  "Player",
  { name: DataTypes.STRING },
  { timestamps: false }
);
const Game = sequelize.define(
  "Game",
  { date: DataTypes.DATE, winnerId: DataTypes.INTEGER },
  { timestamps: false }
);
const GamePlayer = sequelize.define(
  "GamePlayer",
  { gameId: DataTypes.INTEGER, playerId: DataTypes.INTEGER },
  { timestamps: false }
);

async function populateDB() {
  try {
    await sequelize.sync({ force: true }); // This will clear the database

    const playerInstances = await Player.bulkCreate(
      players.map((name) => ({ name }))
    );

    const games = [];
    for (let i = 0; i < 10; i++) {
      const date = new Date();
      date.setFullYear(date.getFullYear() - Math.floor(Math.random() * 2));
      date.setDate(date.getDate() - Math.floor(Math.random() * 365));
      const winnerId =
        playerInstances[Math.floor(Math.random() * playerInstances.length)].id;
      games.push({ date, winnerId });
    }
    const gameInstances = await Game.bulkCreate(games);

    for (const game of gameInstances) {
      const playerIds = playerInstances.map((p) => p.id);
      while (playerIds.length) {
        const playerId = playerIds.splice(
          Math.floor(Math.random() * playerIds.length),
          1
        )[0];
        await GamePlayer.create({ gameId: game.id, playerId });
      }
    }

    console.log("Database populated successfully");
  } catch (err) {
    console.error("Error populating database:", err);
  } finally {
    await sequelize.close();
  }
}

populateDB();
