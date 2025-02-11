using Position = (int, int);
using Direction = (int, int);

enum Type {
    WALL,
    PATH,
    START,
    END,
    OTHER,
}

class Program {
    static readonly Direction NORTH = (-1, 0);
    static readonly Direction SOUTH = (1, 0);
    static readonly Direction WEST = (0, -1);
    static readonly Direction EST = (0, 1);
    static readonly Direction[] DIRECTIONS = {NORTH, SOUTH, WEST, EST};

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static void draw(List<List<Type>> grid) {
        foreach (List<Type> row in grid) {
            foreach (Type type in row) {
                switch (type) {
                    case Type.WALL:
                        Console.Write('#');
                        break;
                    case Type.PATH:
                        Console.Write('.');
                        break;
                    case Type.START:
                        Console.Write('S');
                        break;
                    case Type.END:
                        Console.Write('E');
                        break;
                    case Type.OTHER:
                        Console.Write('!');
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    static void draw(int[,] distances, int rows, int columns) {
        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                Console.Write("{0,5} ", distances[i, j]);
            }
            Console.WriteLine();
        }
    }

    static Position move(Position position, Direction direction) {
        return (position.Item1 + direction.Item1, position.Item2 + direction.Item2);
    }

    static List<Direction> getPossible(List<List<Type>> grid, Position position) {
        List<Direction> output = new();
        int x, y;

        foreach (Direction direction in DIRECTIONS) {
            (x, y) = move(position, direction);

            if (grid[x][y] == Type.PATH) output.Add(direction);
        }

        return output;
    }

    static int[,] buildDistanceGrid(List<List<Type>> grid) {
        int rows = grid.Count;
        int columns = grid[0].Count;
        int[,] distances = new int[rows, columns];

        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                switch (grid[i][j]) {
                    case Type.START:
                        distances[i, j] = 0;
                        break;
                    case Type.END:
                    case Type.PATH:
                        distances[i, j] = int.MaxValue;
                        break;
                    case Type.WALL:
                        distances[i, j] = -1;
                        break;
                }
            }
        }

        return distances;
    }

    static int[,] search(List<List<Type>> grid, Position start) {
        Position currentPosition, next;
        Direction currentDirection;
        int xCurrent, yCurrent, xNext, yNext;
        int[,] distances = buildDistanceGrid(grid);
        Queue<(Position, Direction)> toVisit = new();
        toVisit.Enqueue((start, EST));

        while (toVisit.Count != 0) {
            (currentPosition, currentDirection) = toVisit.Dequeue();
            (xCurrent, yCurrent) = currentPosition;

            foreach (Direction direction in DIRECTIONS) {
                next = move(currentPosition, direction);
                (xNext, yNext) = next;

                if (grid[xNext][yNext] == Type.WALL) continue;

                if (distances[xNext, yNext] == int.MaxValue) toVisit.Enqueue((next, direction));

                if (distances[xNext, yNext] > 1 + distances[xCurrent, yCurrent]) {
                    toVisit.Enqueue((next, direction));
                    if (currentDirection == direction) {
                        distances[xNext, yNext] = 1 + distances[xCurrent, yCurrent];
                    } else {
                        distances[xNext, yNext] = 1000 + 1 + distances[xCurrent, yCurrent];
                    }
                }
            }
        }

        return distances;
    }

    static void part1(string filename) {
        int output = 0;
        int verticesCount = 0;
        Position start = (-1, -1);
        int xEnd = -1;
        int yEnd = -1;
        int rows, columns;
        List<List<Type>> grid = new();
        HashSet<Position> visited = new();
        int[,] distances;

        using(StreamReader reader = File.OpenText(filename)) {
            while (!reader.EndOfStream) {
                grid.Add(reader
                        .ReadLine()!
                        .ToCharArray()
                        .Select(
                            x => x switch {
                            '#' => Type.WALL,
                            '.' => Type.PATH,
                            'S' => Type.START,
                            'E' => Type.END,
                            _   => Type.OTHER,
                            }
                            )
                        .ToList()
                        );
            }
        }

        rows = grid.Count;
        columns = grid[0].Count;

        for (int i=0; i < rows; i++) {
            for (int j=0; j < columns; j++) {
                if (grid[i][j] == Type.START) start = (i, j);
                if (grid[i][j] == Type.END) (xEnd, yEnd) = (i, j);
                if (grid[i][j] != Type.WALL) verticesCount++;
            }
        }

        draw(grid);
        distances = search(grid, start);
        draw(distances, rows, columns);
        output = distances[xEnd, yEnd];

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        int output = 0;

        Console.WriteLine("part2: " + output);
    }
}
