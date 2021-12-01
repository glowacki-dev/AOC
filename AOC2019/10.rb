require "matrix"
require "set"

input = %q[
.#.####..#.#...#...##..#.#.##.
..#####.##..#..##....#..#...#.
......#.......##.##.#....##..#
..#..##..#.###.....#.#..###.#.
..#..#..##..#.#.##..###.......
...##....#.##.#.#..##.##.#...#
.##...#.#.##..#.#........#.#..
.##...##.##..#.#.##.#.#.#.##.#
#..##....#...###.#..##.#...##.
.###.###..##......#..#...###.#
.#..#.####.#..#....#.##..#.#.#
..#...#..#.#######....###.....
####..#.#.#...##...##....#..##
##..#.##.#.#..##.###.#.##.##..
..#.........#.#.#.#.......#..#
...##.#.....#.#.##........#..#
##..###.....#.............#.##
.#...#....#..####.#.#......##.
..#..##..###...#.....#...##..#
...####..#.#.##..#....#.#.....
####.#####.#.#....#.#....##.#.
#.#..#......#.........##..#.#.
#....##.....#........#..##.##.
.###.##...##..#.##.#.#...#.#.#
##.###....##....#.#.....#.###.
..#...#......#........####..#.
#....#.###.##.#...#.#.#.#.....
.........##....#...#.....#..##
###....#.........#..#..#.#.#..
##...#...###.#..#.###....#.##.
].strip

width = input.lines.first.strip.chars.count
height = input.lines.count
asteroids = []

input.lines.each.with_index do |line, y|
  line.strip.chars.each.with_index do |char, x|
    next if char != "#"

    asteroids << Vector[x, y]
  end
end

detects = {}

asteroids.each do |base|
  detects[base] = {}
  asteroids.select { |asteroid| asteroid != base }.each do |asteroid|
    v = base - asteroid
    y = Vector[0, 1]
    dot = v.dot(y)
    det = Matrix[v, y].det
    angle = Math.atan2(det, dot)
    detects[base][angle] ? detects[base][angle] << asteroid : detects[base][angle] = [asteroid]
  end
end

base, targets = detects.max_by { |k, v| v.count }

puts base
puts targets.count

targets.each do |key, values|
  targets[key] = values.sort { |v| (base - v).norm }
end

angles = targets.keys.sort.reverse.lazy.cycle.drop_while { |angle| angle > 0 }

cnt = 0

loop do
  break if targets.count == 0 || cnt == 200

  angle = angles.next
  next unless targets[angle]

  vector = targets[angle].shift
  cnt += 1
  puts "Firing ##{cnt} at #{angle} => #{vector}"
  if targets[angle].count == 0
    puts "Getting rid of #{angle}"
    targets.delete(angle)
  end
end
