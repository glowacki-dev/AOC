var _ = require("lodash");
const graphology = require("graphology");
const path = require("path");

class Solver {
  constructor(data) {
    this.data = [];
    data.forEach((line) => {
      if (line !== "") this.data.push(line.split(" "));
    });
    this.graph = new graphology.DirectedGraph();
  }

  run() {
    var score = 0;
    var current_path = "";
    this.graph.addNode("/", { type: "dir", size: 0 });
    this.graph.on("nodeAttributesUpdated", ({ key, attributes }) => {
      if (key === "/") return;
      // recalculate size for parent node
      var new_parent_size = _.sum(
        this.graph.mapOutNeighbors(path.dirname(key), (key, attributes) => {
          return Number(attributes.size);
        })
      );
      this.graph.setNodeAttribute(path.dirname(key), "size", new_parent_size);
    });
    this.data.forEach((command) => {
      switch (command[0]) {
        case "$":
          switch (command[1]) {
            case "cd":
              current_path = path.join(current_path, command[2]);
              console.log("Moved to", current_path);
              break;
            case "ls":
              console.log("List");
              break;
          }
          break;
        case "dir":
          var new_path = path.join(current_path, command[1]);
          console.log("Adding directory", command[1]);
          this.graph.addNode(new_path, { type: "dir", size: 0 });
          this.graph.addEdge(current_path, new_path);
          break;
        default:
          var new_path = path.join(current_path, command[1]);
          console.log("Adding file", command[1], command[0]);
          this.graph.addNode(new_path, { type: "file", size: 0 });
          this.graph.addEdge(current_path, new_path);
          this.graph.setNodeAttribute(new_path, "size", Number(command[0]));
      }
    });
    console.log("graph");
    this.graph.forEachNode((node, attributes) => {
      console.log(node, attributes);
      if (attributes.type === "dir" && attributes.size <= 100000)
        score += attributes.size;
    });
    return score;
  }
}

module.exports = Solver;
