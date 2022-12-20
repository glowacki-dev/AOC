var _ = require("lodash");

class Solver {
  constructor(data, preview) {
    this.numbers = [];
    this.preview = preview;
    data.forEach((line, index) => {
      if (line !== "") {
        this.numbers.push({ value: Number(line) * 811589153, index });
      }
    });
    this.preview(this.numbers);
  }

  mix() {
    for (let i = 0; i < this.numbers.length; i++) {
      let index = _.findIndex(this.numbers, { index: i });
      let el = this.numbers[index];
      this.numbers.splice(index, 1);
      let newIndex = (index + el.value) % this.numbers.length;
      if (newIndex === 0) newIndex = this.numbers.length;
      this.preview([i, "Add", el.value, "at", newIndex].join(" "));
      this.numbers.splice(newIndex, 0, el);
      this.preview(this.numbers);
    }
  }

  run() {
    for (let i = 0; i < 10; i++) this.mix();
    let start = _.findIndex(this.numbers, { value: 0 });
    this.preview(start);
    const indices = [1000, 2000, 3000];
    return _.sum(
      indices
        .map((i) => this.numbers[(i + start) % this.numbers.length])
        .map((n) => n.value)
    );
  }
}

module.exports = Solver;
