with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")

colors = {
        "red": 12,
        "green": 13,
        "blue": 14,
        }

output = 0


for line in lines:
    gameID, game = line.split(":")
    subsets = game.split(";")
    resForSubset = True

    for subset in subsets:
        cubes = subset.strip().split(",")
        cubes = list(map(lambda x: x.strip().split(" "), cubes))

        if not all(map(lambda x: int(x[0]) <= colors[x[1]], cubes)):
            resForSubset = False
            break

    if resForSubset:
        output += int(gameID.split(" ")[1])

print(output)
