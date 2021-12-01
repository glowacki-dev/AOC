input = %q[
<x=-7, y=-8, z=9>
<x=-12, y=-3, z=-4>
<x=6, y=-17, z=-9>
<x=4, y=-10, z=-6>
].strip

class Moon
  attr_reader :x, :y, :z, :dx, :dy, :dz

  def initialize(x, y, z)
    @x = x
    @y = y
    @z = z
    @dx = 0
    @dy = 0
    @dz = 0
  end

  def to_s
    "pos=<x=#{x}, y=#{y}, z=#{z}>, vel=<x=#{dx}, y=#{dy}, z=#{dz}>"
  end

  def gravitate_towards(moon)
    @dx -= x <=> moon.x
    @dy -= y <=> moon.y
    @dz -= z <=> moon.z
  end

  def move
    @x += dx
    @y += dy
    @z += dz
  end

  def energy
    (x.abs + y.abs + z.abs) * (dx.abs + dy.abs + dz.abs)
  end
end

moons = input.lines.map do |line|
  x, y, z = line.match(/<x=([-\d]+), y=([-\d]+), z=([-\d]+)>/).to_a[1..3].map(&:to_i)
  Moon.new(x, y, z)
end

states = { x: {}, y: {}, z: {} }

states[:x][moons.map { |m| [m.x, m.dx] }.hash] = 0
states[:y][moons.map { |m| [m.y, m.dy] }.hash] = 0
states[:z][moons.map { |m| [m.z, m.dz] }.hash] = 0
sx = 0
sy = 0
sz = 0

(1..Float::INFINITY).each do |i|
  moons.permutation(2).each { |a, b| a.gravitate_towards(b) }
  moons.each { |moon| moon.move }
  if sx == 0
    h = moons.map { |m| [m.x, m.dx] }.hash
    if states[:x].key?(h)
      puts "X after #{i} steps"
      sx = i
    end
    states[:x][h] = i
  end
  if sy == 0
    h = moons.map { |m| [m.y, m.dy] }.hash
    if states[:y].key?(h)
      puts "Y after #{i} steps"
      sy = i
    end
    states[:y][h] = i
  end
  if sz == 0
    h = moons.map { |m| [m.z, m.dz] }.hash
    if states[:z].key?(h)
      puts "Z after #{i} steps"
      sz = i
    end
    states[:z][h] = i
  end
  if sx != 0 && sy != 0 && sz != 0
    break
  end
end

puts sx
puts sy
puts sz

puts [sx, sy, sz].reduce(:lcm)
