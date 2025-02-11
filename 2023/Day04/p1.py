with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")

output = 0

for line in lines:
    line = line.replace("  ", " ")
    cardID, numbers = line.split(":")
    validNumbers, myNumbers = numbers.strip().split("|")
    validNumbers = validNumbers.strip().split(" ")
    myNumbers = myNumbers.strip().split(" ")

    points = 0

    for n in myNumbers:
        if n in validNumbers:
            if not points:
                points = 1
            else:
                points *= 2

    output += points

print(output)
