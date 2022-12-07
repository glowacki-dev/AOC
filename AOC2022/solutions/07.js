var _ = require("lodash");
const graphology = require("graphology");
const path = require("path");

class Solver {
  constructor(data, preview) {
    this.data = [];
    this.preview = preview;
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
              this.preview("Moved to", current_path);
              break;
            case "ls":
              this.preview("List");
              break;
          }
          break;
        case "dir":
          var new_path = path.join(current_path, command[1]);
          this.preview("Adding directory", command[1]);
          this.graph.addNode(new_path, { type: "dir", size: 0 });
          this.graph.addEdge(current_path, new_path);
          break;
        default:
          var new_path = path.join(current_path, command[1]);
          this.preview("Adding file", command[1], command[0]);
          this.graph.addNode(new_path, { type: "file", size: 0 });
          this.graph.addEdge(current_path, new_path);
          this.graph.setNodeAttribute(new_path, "size", Number(command[0]));
      }
    });
    const minimum_freeable =
      30000000 - (70000000 - this.graph.getNodeAttributes("/").size);
    this.preview("Looking for", minimum_freeable);
    var deletion_candidates = [];
    this.graph.forEachNode((node, attributes) => {
      if (attributes.type === "dir") this.preview(node, attributes);
      if (attributes.type === "dir" && attributes.size >= minimum_freeable)
        deletion_candidates.push(attributes.size);
    });
    this.preview(deletion_candidates);
    score = _.min(deletion_candidates);
    return score;
  }
}

module.exports = Solver;
