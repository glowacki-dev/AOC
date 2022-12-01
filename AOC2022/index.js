const DAY = process.env.DAY;
const SAMPLE = process.env.SAMPLE || false;

const fs = require("fs");
const Solver = require(`./solutions/${DAY}.js`);
const data_path = SAMPLE ? `./data/samples/${DAY}.txt` : `./data/${DAY}.txt`;

fs.readFile(data_path, "utf8", (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  var lines = data.split("\n");
  new Solver(lines).run();
});
