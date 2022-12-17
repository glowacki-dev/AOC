var _ = require("lodash");
var Queue = require("mnemonist/queue");

class Solver {
  constructor(data, preview) {
    this.rocks = new Queue.of(
      [0b0011110],
      [0b0001000, 0b0011100, 0b0001000],
      [0b0011100, 0b0000100, 0b0000100],
      [0b0010000, 0b0010000, 0b0010000, 0b0010000],
      [0b0011000, 0b0011000]
    );
    this.moves = new Queue();
    this.map = [0b1111111];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "")
        line.split("").map((val) => {
          this.moves.enqueue(val);
        });
    });
  }

  simulateRock() {
    let rock = this.rocks.dequeue();
    this.rocks.enqueue(rock);
    rock = _.cloneDeep(rock);
    let height = this.map.length + 3;
    while (true) {
      // Move to sides
      let nextMove = this.moves.dequeue();
      this.moves.enqueue(nextMove);
      switch (nextMove) {
        case "<":
          if (
            _.every(
              rock,
              (val, idx) =>
                (val & 0b1000000) === 0 &&
                ((val << 1) & (this.map[height + idx] || 0)) === 0
            )
          ) {
            rock = _.map(rock, (val) => val << 1);
          }
          break;
        case ">":
          if (
            _.every(
              rock,
              (val, idx) =>
                (val & 0b0000001) === 0 &&
                ((val >> 1) & (this.map[height + idx] || 0)) === 0
            )
          ) {
            rock = _.map(rock, (val) => val >> 1);
          }
          break;
      }
      // Try to move down
      // Add to map if move not possible
      for (let i = 0; i < rock.length; i++) {
        // check if any individual rock segment can move down
        let below = this.map[height - 1 + i];
        if (below !== undefined) {
          // Some part will collide, finish simulation
          if ((rock[i] & below) !== 0) {
            rock.forEach((val, idx) => {
              if (this.map[height + idx] !== undefined) {
                this.map[height + idx] |= val;
              } else {
                this.map.push(val);
              }
            });
            return;
          }
        }
      }
      // Nothing collided, continue move
      height -= 1;
    }
  }

  run() {
    for (let i = 0; i < 2022; i++) {
      this.simulateRock();
    }
    this.preview(
      _.reverse(
        this.map.map((val) => (val | 0b10000000).toString(2) + "1")
      ).join("\n")
    );
    return this.map.length - 1;
  }
}

module.exports = Solver;
