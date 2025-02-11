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

    dico = {}

    for subset in subsets:
        cubes = subset.strip().split(",")
        cubes = list(map(lambda x: x.strip().split(" "), cubes))

        for x, y in cubes:
            dico[y] = max(int(x), dico.get(y, 0))

    output += dico["red"] * dico["green"] * dico["blue"]

print(output)
