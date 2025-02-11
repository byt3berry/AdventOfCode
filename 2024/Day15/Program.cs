using Position = (int, int);
using Action = (int, int);

enum Type {
    EMPTY,
    BOX,
    BIG_BOX_LEFT,
    BIG_BOX_RIGHT,
    WALL,
    ROBOT,
    OTHER,
}

class Program {
    static Action UP = (-1, 0);
    static Action DOWN = (1, 0);
    static Action LEFT = (0, -1);
    static Action RIGHT = (0, 1);

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static Position move(Position position, Action action) {
        return (position.Item1 + action.Item1, position.Item2 + action.Item2);
    }

    static Position move(ref List<List<Type>> grid, Position position, Action action) {
        int otherX, otherY, otherNewX, otherNewY;
        Position otherBox, otherNextPosition;
        Position nextPosition = move(position, action);
        var (x, y) = position;
        var (nextX, nextY) = nextPosition;

        switch (grid[x][y]) {
            case Type.EMPTY:
            case Type.WALL:
                break;
            case Type.BIG_BOX_LEFT:
                otherBox = move(position, RIGHT);
                otherNextPosition = move(otherBox, action);
                (otherX, otherY) = otherBox;
                (otherNewX, otherNewY) = otherNextPosition;

                if (action == UP || action == DOWN) {
                    move(ref grid, nextPosition, action);
                    move(ref grid, otherNextPosition, action);
                    grid[nextX][nextY] = grid[x][y];
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                    grid[x][y] = Type.EMPTY;
                    grid[otherX][otherY] = Type.EMPTY;
                } else if (action == LEFT) {
                    move(ref grid, nextPosition, action);
                    grid[nextX][nextY] = grid[x][y];
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                } else if (action == RIGHT) {
                    move(ref grid, otherNextPosition, action);
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                    grid[nextX][nextY] = grid[x][y];
                }

                break;
            case Type.BIG_BOX_RIGHT:
                otherBox = move(position, LEFT);
                otherNextPosition = move(otherBox, action);
                (otherX, otherY) = otherBox;
                (otherNewX, otherNewY) = otherNextPosition;

                if (action == UP || action == DOWN) {
                    move(ref grid, nextPosition, action);
                    move(ref grid, otherNextPosition, action);
                    grid[nextX][nextY] = grid[x][y];
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                    grid[x][y] = Type.EMPTY;
                    grid[otherX][otherY] = Type.EMPTY;
                } else if (action == LEFT) {
                    move(ref grid, otherNextPosition, action);
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                    grid[nextX][nextY] = grid[x][y];
                } else if (action == RIGHT) {
                    move(ref grid, nextPosition, action);
                    grid[nextX][nextY] = grid[x][y];
                    grid[otherNewX][otherNewY] = grid[otherX][otherY];
                }

                break;
            case Type.BOX:
            case Type.ROBOT:
                move(ref grid, nextPosition, action);
                grid[nextX][nextY] = grid[x][y];
                grid[x][y] = Type.EMPTY;
                break;
        }

        return nextPosition;
    }

    static bool canMove(List<List<Type>> grid, Position position, Action action) {
        Position otherBox, otherNextPosition;
        var (x, y) = position;
        Position nextPosition = move(position, action);
        var (newX, newY) = nextPosition;

        switch (grid[x][y]) {
            case Type.EMPTY:
                return true;
            case Type.WALL:
                return false;
            case Type.BOX:
            case Type.ROBOT:
                if (canMove(grid, nextPosition, action)) return true;
                else return false;
            case Type.BIG_BOX_LEFT:
                otherBox = move(position, RIGHT);
                if (action == UP || action == DOWN) {
                    otherNextPosition = move(otherBox, action);
                    return canMove(grid, nextPosition, action) && canMove(grid, otherNextPosition, action);
                } else if (action == LEFT) {
                    return canMove(grid, nextPosition, action);
                } else if (action == RIGHT) {
                    return canMove(grid, otherBox, action);
                }
                return false;
            case Type.BIG_BOX_RIGHT:
                otherBox = move(position, LEFT);
                if (action == UP || action == DOWN) {
                    otherNextPosition = move(otherBox, action);
                    return canMove(grid, nextPosition, action) && canMove(grid, otherNextPosition, action);
                } else if (action == LEFT) {
                    return canMove(grid, otherBox, action);
                } else if (action == RIGHT) {
                    return canMove(grid, nextPosition, action);
                }
                return false;
        }

        return false;
    }

    static void draw(List<List<Type>> grid) {
        foreach (List<Type> row in grid) {
            foreach (Type obj in row) {
                switch (obj) {
                    case Type.EMPTY:
                        Console.Write('.');
                        break;
                    case Type.WALL:
                        Console.Write('#');
                        break;
                    case Type.BOX:
                        Console.Write('O');
                        break;
                    case Type.BIG_BOX_LEFT:
                        Console.Write('[');
                        break;
                    case Type.BIG_BOX_RIGHT:
                        Console.Write(']');
                        break;
                    case Type.ROBOT:
                        Console.Write('@');
                        break;
                    default:
                        Console.Write('!');
                        break;
                }
                
            }
            Console.WriteLine();
        }
    }

    static int computeSum(List<List<Type>> grid) {
        int sum = 0;

        for (int i=0; i < grid.Count(); i++) {
            for (int j=0; j < grid[i].Count(); j++) {
                if (grid[i][j] == Type.BOX) {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }

    static void part1(string filename) {
        int output;
        List<List<Type>> grid = new();
        List<Action> actions = new();
        Position robot = (-1, -1);

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != "") {
                grid.Add(line!
                        .ToCharArray()
                        .Select(c => c switch {
                            '.' => Type.EMPTY,
                            'O' => Type.BOX,
                            '#' => Type.WALL,
                            '@' => Type.ROBOT,
                            _ => Type.OTHER,
                            })
                        .ToList()
                        );
                line = reader.ReadLine();
            }

            while (!reader.EndOfStream) {
                actions.AddRange(reader
                        .ReadLine()!
                        .ToCharArray()
                        .Select(c => c switch {
                            '^' => UP,
                            'v' => DOWN,
                            '<' => LEFT,
                            '>' => RIGHT,
                            _ => (0, 0),
                            })
                        .ToList()
                        );
            }
        }

        for (int i=0; i < grid.Count; i++) {
            for (int j=0; j < grid[i].Count; j++) {
                if (grid[i][j] == Type.ROBOT) robot = (i, j);
            }
        }

        foreach (Action action in actions) {
            if (canMove(grid, robot, action)) {
                robot = move(ref grid, robot, action);
            }
        }

        output = computeSum(grid);

        Console.WriteLine("part1: " + output);
    }

    static int computeBigSum(List<List<Type>> grid) {
        int sum = 0;

        for (int i=0; i < grid.Count(); i++) {
            for (int j=0; j < grid[i].Count(); j++) {
                if (grid[i][j] == Type.BIG_BOX_LEFT) {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }

    static void part2(string filename) {
        int output;
        List<List<Type>> grid = new();
        List<Action> actions = new();
        Position robot = (-1, -1);

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != "") {
                grid.Add(line!
                        .ToCharArray()
                        .SelectMany(c => c switch {
                            '.' => new Type[]{Type.EMPTY, Type.EMPTY},
                            'O' => new Type[]{Type.BIG_BOX_LEFT, Type.BIG_BOX_RIGHT},
                            '#' => new Type[]{Type.WALL, Type.WALL},
                            '@' => new Type[]{Type.ROBOT, Type.EMPTY},
                            _ => new Type[]{Type.OTHER, Type.OTHER},
                            })
                        .ToList()
                        );
                line = reader.ReadLine();
            }

            while (!reader.EndOfStream) {
                actions.AddRange(reader
                        .ReadLine()!
                        .ToCharArray()
                        .Select(c => c switch {
                            '^' => UP,
                            'v' => DOWN,
                            '<' => LEFT,
                            '>' => RIGHT,
                            _ => (0, 0),
                            })
                        .ToList()
                        );
            }
        }

        for (int i=0; i < grid.Count; i++) {
            for (int j=0; j < grid[i].Count; j++) {
                if (grid[i][j] == Type.ROBOT) robot = (i, j);
            }
        }

        foreach (Action action in actions) {
            if (canMove(grid, robot, action)) {
                robot = move(ref grid, robot, action);
            }
        }

        output = computeBigSum(grid);

        Console.WriteLine("part2: " + output);
    }
}
