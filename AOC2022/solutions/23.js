var _ = require("lodash");

// [y, x]
const DIRECTIONS = [
  {
    checks: [
      [-1, -1],
      [-1, 0],
      [-1, 1],
    ],
    move: [-1, 0],
  },
  {
    checks: [
      [1, -1],
      [1, 0],
      [1, 1],
    ],
    move: [1, 0],
  },
  {
    checks: [
      [1, -1],
      [0, -1],
      [-1, -1],
    ],
    move: [0, -1],
  },
  {
    checks: [
      [1, 1],
      [0, 1],
      [-1, 1],
    ],
    move: [0, 1],
  },
];
const AROUND = [
  [-1, -1],
  [-1, 0],
  [-1, 1],
  [0, 1],
  [1, 1],
  [1, 0],
  [1, -1],
  [0, -1],
];

class Solver {
  constructor(data, preview) {
    this.map = new Set();
    this.preview = preview;
    data.forEach((line, y) => {
      if (line !== "") {
        line.split("").forEach((symbol, x) => {
          if (symbol === "#") {
            this.map.add(`${y},${x}`);
          }
        });
      }
    });
    this.preview(this.map);
  }

  simulate(round) {
    let plans = {};
    this.map.forEach((entry) => {
      let [y, x] = entry.split(",").map((val) => Number(val));
      let arounds = AROUND.map(([dy, dx]) => `${dy + y},${dx + x}`);
      if (_.every(arounds, (key) => !this.map.has(key))) return;
      for (let variant = 0; variant < 4; variant++) {
        let direction = DIRECTIONS[(round + variant) % DIRECTIONS.length];
        let checkPositions = direction.checks.map(
          ([dy, dx]) => `${dy + y},${dx + x}`
        );
        if (_.some(checkPositions, (key) => this.map.has(key))) continue;
        let newKey = `${direction.move[0] + y},${direction.move[1] + x}`;
        if (plans[newKey]) plans[newKey].push({ from: entry });
        else plans[newKey] = [{ from: entry }];
        return;
      }
    });
    this.preview(plans);
    let moves = 0;
    Object.keys(plans).forEach((position) => {
      if (plans[position].length > 1) return;

      moves += 1;
      this.map.add(position);
      this.map.delete(plans[position][0].from);
    });
    return moves;
  }

  render() {
    let minX = Number.MAX_SAFE_INTEGER;
    let minY = Number.MAX_SAFE_INTEGER;
    let maxX = Number.MIN_SAFE_INTEGER;
    let maxY = Number.MIN_SAFE_INTEGER;
    this.map.forEach((entry) => {
      let [y, x] = entry.split(",").map((val) => Number(val));
      if (y < minY) minY = y;
      if (y > maxY) maxY = y;
      if (x < minX) minX = x;
      if (x > maxX) maxX = x;
    });
    let empties = 0;
    for (let y = minY; y <= maxY; y++) {
      let line = "";
      for (let x = minX; x <= maxX; x++) {
        if (!this.map.has(`${y},${x}`)) {
          empties += 1
          line += ".";
        } else {
          line += "#";
        }
      }
      this.preview(line);
    }
    return empties;
  }

  run() {
    let score = 0;
    for (let i = 0; ; i++) {
      this.render();
      let moves = this.simulate(i);
      this.preview(this.map);
      this.preview("");
      if (moves === 0) {
        score = i + 1;
        break;
      }
    }
    return score;
  }
}

module.exports = Solver;
