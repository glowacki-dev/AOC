min = 178416
max = 676461

c = (min..max).lazy.map { |v| v.to_s.split("").map(&:to_i) }.select do |v|
  v.each_cons(2).all? { |a, b| a <= b }
end.select do |v|
  v.uniq != v
end.select do |v|
  v.group_by(&:itself).transform_values(&:count).values.any? { |vv| vv == 2 }
end.count

puts c