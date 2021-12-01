@data = [
1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,9,1,19,1,19,5,23,1,23,6,27,2,9,27,31,1,5,31,35,1,35,10,39,1,39,10,43,2,43,9,47,1,6,47,51,2,51,6,55,1,5,55,59,2,59,10,63,1,9,63,67,1,9,67,71,2,71,6,75,1,5,75,79,1,5,79,83,1,9,83,87,2,87,10,91,2,10,91,95,1,95,9,99,2,99,9,103,2,10,103,107,2,9,107,111,1,111,5,115,1,115,2,119,1,119,6,0,99,2,0,14,0
]

def process(input, index)
  case input[index]
  when 1
    input[input[index + 3]] = input[input[index + 1]] + input[input[index + 2]]
    return input, index + 4
  when 2
    input[input[index + 3]] = input[input[index + 1]] * input[input[index + 2]]
    return input, index + 4
  when 99
    return input, -1
  else
    raise StandardError, "ded"
  end
end


def compute(input)
  index = 0
  while(index >= 0) do
    input, index = process(input, index)
  end
  return input[0]
end

# compute(@data)

(0..99).to_a.permutation(2).each do |noun, verb|
  input = @data.dup
  input[1] = noun
  input[2] = verb
  result = compute(input)
  if result == 19690720
    puts "Gotcha: #{noun}, #{verb}"
    break
  end
end
