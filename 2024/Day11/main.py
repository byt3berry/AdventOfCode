def blink(stone, n, stored):
    if n == 0: return 1
    if (stone, n) in stored: return stored[(stone, n)];

    result = 0;

    if stone == 0:
        result = blink(1, n-1, stored)
    elif len(str(stone)) % 2 == 0:
        stoneString = str(stone)
        half = len(stoneString) // 2
        left = int(stoneString[:half]);
        right = int(stoneString[half:]);

        leftResult = blink(left, n-1, stored);
        if (left, n-1) not in stored:
            stored[(left, n-1)] = leftResult

        rightResult = blink(right, n-1, stored);
        if (right, n-1) not in stored:
            stored[(right, n-1)] = rightResult

        result = leftResult + rightResult;
    else:
        result = blink(stone * 2024, n-1, stored)

    if (stone, n) not in stored:
        stored[(stone, n)] = result

    return result

with open("input.txt", "r") as f:
    stones = [int(x) for x in f.read().strip().split(" ")]

output = 0;
stored = {}

for stone in stones:
    output += blink(stone, 75, stored);

print("part2:", output);

