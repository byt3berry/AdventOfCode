numbers = {
        "one": "1",
        "two": "2",
        "three": "3",
        "four": "4",
        "five": "5",
        "six": "6",
        "seven": "7",
        "eight": "8",
        "nine": "9",
        }

with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")

sum = 0

for line in lines:
    chars = list(line)
    first, last = -1, -1
    buffer = ""

    for i, char in enumerate(chars):
        if '0' <= char <= '9':
            if first == -1:
                first = int(char)

            last = int(char)
            buffer = ""
        else:
            buffer += char

            for n in numbers:
                if buffer.endswith(n):
                    if first == -1:
                        first = int(numbers[n])

                    last = int(numbers[n])

    sum += first * 10 + last


print(sum)
