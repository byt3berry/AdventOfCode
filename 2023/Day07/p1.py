from collections import Counter


with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")

lines = [
        line.split(" ")
        for line in lines
        ]

order = "23456789TJQKA"
score = {}


def getHandType(hand):
    if score[hand].get("handType", None) is not None:
        return score[hand]["handType"]

    counter = Counter(hand)

    if len(counter.keys()) == 1:
        score[hand]["handType"] = 7

    elif len(counter.keys()) == 2:
        keys = list(counter.keys())

        if any(counter.get(x) == 3 for x in keys):
            score[hand]["handType"] = 5
        else:
            score[hand]["handType"] = 6

    elif len(counter.keys()) == 3:
        keys = list(counter.keys())

        if any(counter.get(x) == 3 for x in keys):
            score[hand]["handType"] = 4
        else:
            score[hand]["handType"] = 3

    elif len(counter.keys()) == 4:
        score[hand]["handType"] = 2

    elif len(counter.keys()) == 5:
        score[hand]["handType"] = 1

    return score[hand]["handType"]


# return if hand1 > hand2
def isBetter(hand1, hand2):
    handType1 = getHandType(hand1)
    handType2 = getHandType(hand2)

    if handType1 != handType2:
        return handType1 > handType2

    hand1 = list(hand1)
    hand2 = list(hand2)

    for i in range(5):
        if order.find(hand1[i]) != order.find(hand2[i]):
            return order.find(hand1[i]) > order.find(hand2[i])


output = 0

for hand, bid in lines:
    score[hand] = {
            "bid": bid,
            "wins": 0,
            }

for i in range(len(lines)):
    hand1, _ = lines[i]

    for j in range(i+1, len(lines)):
        hand2, _ = lines[j]

        if isBetter(hand1, hand2):
            score[hand1]["wins"] += 1
        else:
            score[hand2]["wins"] += 1


for _, dico in score.items():
    output += (dico["wins"] + 1) * int(dico["bid"])

print(output)
