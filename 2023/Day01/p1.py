with open("input.txt", "r") as f:
    lines = f.read().strip().split("\n")


sum = 0

for line in lines:
    chars = list(line)
    first, last = -1, -1

    for char in chars:
        if '0' <= char <= '9':
            if first == -1:
                first = int(char)

            last = int(char)

    sum += first * 10 + last


print(sum)
