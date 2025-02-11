with open("input.txt", "r") as f:
    steps = f.read().strip().split("\n\n")

seeds = list(map(int, steps[0].split(":")[1].strip().split(" ")))
steps = steps[1::]
location = None

for seed in seeds:
    for step in steps:
        step = step.split("\n")
        title = step[0]
        ranges = step[1::]

        for range_ in ranges:
            destinationStart, sourceStart, length = map(int, range_.split(" "))

            if sourceStart <= seed < sourceStart + length:
                offset = seed - sourceStart
                seed = destinationStart + offset
                break

    if location is None or seed < location:
        location = seed

print(location)
