with open("input.txt", "r") as f:
    times, distances = f.read().strip().split("\n")

times = [
        int(x)
        for x in times.split(" ")[1::]
        if x
        ]
distances = [
        int(x)
        for x in distances.split(" ")[1::]
        if x
        ]

output = 1

for race in range(len(times)):
    time = times[race]
    distance = distances[race]
    possibilities = 0

    for timeHolding in range(time):
        timeMoving = time - timeHolding
        distanceMoved = timeHolding * timeMoving

        if distanceMoved > distance:
            possibilities += 1

    output *= possibilities

print(output)
