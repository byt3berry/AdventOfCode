from pprint import pprint


with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")


def get(i, j):
    if 0 <= i < len(lines) and 0 <= j < len(lines[i]):
        return lines[i][j]

    return "."


def isSymbol(char: str):
    return char != "." and not ("0" <= char <= "9")


def isGear(char: str):
    return char == "*"


def findGearsAround(i, j):
    output = set()

    for m in (-1, 0, 1):
        for n in (-1, 0, 1):
            if (m, n) == (0, 0):
                continue

            if isGear(get(i + m, j + n)):
                output.add((i + m, j + n))

    return output


gears = {}
output = 0

for i in range(len(lines)):
    buffer = ""
    gearsAround = set()

    for j in range(len(lines[i])):
        char = get(i, j)

        if isSymbol(char) or char == ".":
            for gear in gearsAround:
                gears[f"{gear[0]}:{gear[1]}"] = gears.get(f"{gear[0]}:{gear[1]}", []) + [buffer]

            buffer = ""
            gearsAround = set()
            continue

        buffer += char
        gearsAround = gearsAround.union(findGearsAround(i, j))

    for gear in gearsAround:
        gears[f"{gear[0]}:{gear[1]}"] = gears.get(f"{gear[0]}:{gear[1]}", []) + [buffer]

for _, value in gears.items():
    if len(value) == 2:
        output += int(value[0]) * int(value[1])

print(output)
