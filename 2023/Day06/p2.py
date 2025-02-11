with open("input.txt", "r") as f:
    times, distances = f.read().strip().split("\n")

times = int("".join(times.split(" ")[1::]))
distances = int("".join(distances.split(" ")[1::]))

output = 0

for timeHolding in range(times):
    timeMoving = times - timeHolding
    distanceMoved = timeHolding * timeMoving

    if distanceMoved > distances:
        output += 1

print(output)
