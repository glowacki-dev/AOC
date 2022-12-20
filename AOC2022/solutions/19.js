var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    this.blueprints = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "") {
        let [
          id,
          oreOre,
          clayOre,
          obsidianOre,
          obsidianClay,
          geodeOre,
          geodeObsidian,
        ] = line.match(/\d+/g).map((val) => Number(val));
        this.blueprints.push({
          id,
          ore: { ore: oreOre },
          clay: { ore: clayOre },
          obsidian: { ore: obsidianOre, clay: obsidianClay },
          geode: { ore: geodeOre, obsidian: geodeObsidian },
        });
      }
    });
    this.preview(this.blueprints);
  }

  mine(blueprint) {
    let targets = { geode: 1 };
    targets.obsidian = blueprint.geode.obsidian * targets.geode;
    targets.clay = blueprint.obsidian.clay * targets.obsidian;
    targets.ore =
      blueprint.clay.ore * targets.clay +
      blueprint.obsidian.ore * targets.obsidian +
      blueprint.geode.ore * targets.geode;
    console.log(targets);
  }

  run() {
    let score = 0;
    this.blueprints.forEach((blueprint) => {
      let output = this.mine(blueprint);
      score += output * blueprint.id;
    });
    return score;
  }
}

module.exports = Solver;
