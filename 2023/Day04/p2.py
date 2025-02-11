with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")

cardInstances = [1] * len(lines)

for line in lines:
    line = line.replace("  ", " ")
    cardID, numbers = line.split(":")
    cardNumber = int(cardID[5::]) - 1
    validNumbers, myNumbers = numbers.strip().split("|")
    validNumbers = validNumbers.strip().split(" ")
    myNumbers = myNumbers.strip().split(" ")

    nbMatching = 0

    for n in myNumbers:
        if n in validNumbers:
            nbMatching += 1

    for i in range(1, nbMatching + 1):
        cardInstances[cardNumber + i] += cardInstances[cardNumber]

print(sum(cardInstances))
