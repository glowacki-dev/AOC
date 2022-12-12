var _ = require("lodash");
let createGraph = require("ngraph.graph");
let path = require("ngraph.path");

class Solver {
  constructor(data, preview) {
    this.map = [];
    this.preview = preview;
    data.forEach((line) => {
      if (line !== "")
        this.map.push(
          line.split("").map((val) => {
            if (val === "S")
              return { height: "a".charCodeAt(0), type: "start" };
            if (val === "E") return { height: "z".charCodeAt(0), type: "end" };
            return { height: val.charCodeAt(0) };
          })
        );
    });
    this.preview(this.map);
  }

  run() {
    let graph = createGraph();
    let starts = [];
    let end = null;
    let height = this.map.length;
    let width = this.map[0].length;
    for (let y = 0; y < height; y++) {
      for (let x = 0; x < width; x++) {
        let name = `${x}-${y}`;
        let point = this.map[y][x];
        if (point.height === "a".charCodeAt(0)) {
          starts.push(name);
        } else if (point.type === "end") {
          end = name;
        }
        [
          [-1, 0],
          [0, -1],
          [1, 0],
          [0, 1],
        ].forEach(([dy, dx]) => {
          if (y + dy < 0 || y + dy >= height) return;
          if (x + dx < 0 || x + dx >= width) return;
          let neighbour = this.map[y + dy][x + dx];
          if (neighbour.height <= point.height + 1)
            graph.addLink(name, `${x + dx}-${y + dy}`, { weight: 1 });
        });
      }
    }
    this.preview(starts);
    let distances = starts.map((start) => {
      let pathFinder = path.aStar(graph, {
        distance(fromNode, toNode, link) {
          return link.data.weight;
        },
        oriented: true,
      });
      let foundPath = pathFinder.find(start, end);
      return foundPath.length - 1;
    });
    return _.min(_.filter(distances, (v) => v > 0));
  }
}

module.exports = Solver;
