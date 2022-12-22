var _ = require("lodash");

const DIRECTIONS = [
  [0, 1],
  [1, 0],
  [0, -1],
  [-1, 0],
];

class Solver {
  constructor(data, preview) {
    this.map = {};
    this.commands = [];
    this.player = { direction: 0 };
    let mode = 0;
    this.preview = preview;
    data.forEach((line, lineNumber) => {
      if (line !== "") {
        switch (mode) {
          case 0:
            line.split("").forEach((symbol, idx) => {
              if (symbol === " ") return;

              _.setWith(this.map, `[${lineNumber}][${idx}]`, symbol, Object);
            });
            break;
          case 1:
            this.commands = line.match(/(\d+)|(\D+)/g);
            break;
        }
      } else {
        mode += 1;
      }
    });
    this.player.position = { y: 0, x: Math.min(...Object.keys(this.map[0])) };
    this.preview(this.map);
    this.preview(this.commands);
    this.preview(this.player);
  }

  process(command) {
    this.preview(this.player);
    this.preview(command);
    switch (command) {
      case "L":
        this.player.direction =
          this.player.direction === 0
            ? DIRECTIONS.length - 1
            : this.player.direction - 1;
        break;
      case "R":
        this.player.direction = (this.player.direction + 1) % DIRECTIONS.length;
        break;
      default:
        let val = Number(command);
        let [dy, dx] = DIRECTIONS[this.player.direction];
        for (let i = 0; i < val; i++) {
          let newX = this.player.position.x + dx;
          let newY = this.player.position.y + dy;
          let target = _.get(this.map, [newY, newX], undefined);
          if (target === undefined) {
            if (dx > 0) {
              newX = Number(
                Math.min(...Object.keys(this.map[this.player.position.y]))
              );
            }
            if (dx < 0) {
              newX = Number(
                Math.max(...Object.keys(this.map[this.player.position.y]))
              );
            }
            if (dy > 0) {
              newY = Number(
                _.findKey(this.map, (o) => _.has(o, this.player.position.x))
              );
            }
            if (dy < 0) {
              newY = Number(
                _.findLastKey(this.map, (o) => _.has(o, this.player.position.x))
              );
            }
            this.preview(["Out of bounds", dx, dy, newX, newY]);
          }
          switch (this.map[newY][newX]) {
            case ".":
              this.player.position.y = newY;
              this.player.position.x = newX;
              break;
            case "#":
              break;
          }
        }
        break;
    }
  }

  run() {
    this.commands.forEach((command) => this.process(command));
    this.preview(this.player);
    return (
      1000 * (this.player.position.y + 1) +
      4 * (this.player.position.x + 1) +
      this.player.direction
    );
  }
}

module.exports = Solver;
