with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")


def get(i, j):
    if 0 <= i < len(lines) and 0 <= j < len(lines[i]):
        return lines[i][j]

    return "."


def isSymbol(char: str):
    return char != "." and not char.isdigit()


def findSymbolAround(i, j):
    for m in (-1, 0, 1):
        for n in (-1, 0, 1):
            if (m, n) == (0, 0):
                continue

            if isSymbol(get(i + m, j + n)):
                return True

    return False


output = 0

for i in range(len(lines)):
    buffer = ""
    isEnginePart = False

    for j in range(len(lines[i])):
        char = get(i, j)

        if isSymbol(char) or char == ".":
            if isEnginePart:
                output += int(buffer)

            buffer = ""
            isEnginePart = False
            continue

        buffer += char

        if findSymbolAround(i, j):
            isEnginePart = True

    if isEnginePart:
        output += int(buffer)

print(output)
