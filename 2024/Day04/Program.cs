class Program {
    static readonly string SEARCHED1 = "XMAS";
    static readonly string SEARCHED2 = "MAS";

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool inRange(in List<string> data, in int i, in int j) {
        return 0 <= i && 0 <= j && i < data.Count && j < data[i].Length;
    }


    static int search1(in List<string> data, in int i, in int j) {
        List<int> founds = new(8);

        for (int k=0; k < founds.Capacity; k++) {
            founds.Add(0);
        }

        for (int k=0; k < SEARCHED1.Length; k++) {
            if (inRange(data, i+k, j) && data[i+k][j] == SEARCHED1[k]) {
                founds[0]++;
            }

            if (inRange(data, i-k, j) && data[i-k][j] == SEARCHED1[k]) {
                founds[1]++;
            }

            if (inRange(data, i, j+k) && data[i][j+k] == SEARCHED1[k]) {
                founds[2]++;
            }

            if (inRange(data, i, j-k) && data[i][j-k] == SEARCHED1[k]) {
                founds[3]++;
            }

            if (inRange(data, i+k, j+k) && data[i+k][j+k] == SEARCHED1[k]) {
                founds[4]++;
            }

            if (inRange(data, i+k, j-k) && data[i+k][j-k] == SEARCHED1[k]) {
                founds[5]++;
            }

            if (inRange(data, i-k, j+k) && data[i-k][j+k] == SEARCHED1[k]) {
                founds[6]++;
            }

            if (inRange(data, i-k, j-k) && data[i-k][j-k] == SEARCHED1[k]) {
                founds[7]++;
            }
        }

        return founds.FindAll(x => x == SEARCHED1.Length).Count();
    }

    static void part1(in string filename) {
        int output = 0;
        List<string> data = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                data.Add(line);
                line = reader.ReadLine();
            }
        }

        for (int i=0; i < data.Count; i++) {
            for (int j=0; j < data[i].Length; j++) {
                if (data[i][j] == SEARCHED1[0]) {
                    output += search1(data, i, j);
                }
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static int search2(in List<string> data, in int i, in int j) {
        int output = 0;

        if (
                data[i-1][j-1] == SEARCHED2[0] &&
                data[i-1][j+1] == SEARCHED2[0] &&
                data[i+1][j-1] == SEARCHED2[2] &&
                data[i+1][j+1] == SEARCHED2[2]
           ) {
            output++;
        }

        if (
                data[i-1][j-1] == SEARCHED2[0] &&
                data[i+1][j-1] == SEARCHED2[0] &&
                data[i-1][j+1] == SEARCHED2[2] &&
                data[i+1][j+1] == SEARCHED2[2]
           ) {
            output++;
        }

        if (
                data[i+1][j-1] == SEARCHED2[0] &&
                data[i+1][j+1] == SEARCHED2[0] &&
                data[i-1][j-1] == SEARCHED2[2] &&
                data[i-1][j+1] == SEARCHED2[2]
           ) {
            output++;
        }

        if (
                data[i-1][j+1] == SEARCHED2[0] &&
                data[i+1][j+1] == SEARCHED2[0] &&
                data[i-1][j-1] == SEARCHED2[2] &&
                data[i+1][j-1] == SEARCHED2[2]
           ) {
            output++;
        }

        return output;
    }

    static void part2(in string filename) {
        int output = 0;
        List<string> data = new();

        using(StreamReader reader = File.OpenText(filename)) {
            string? line = reader.ReadLine();

            while (line != null) {
                data.Add(line);
                line = reader.ReadLine();
            }
        }

        for (int i=1; i < data.Count-1; i++) {
            for (int j=1; j < data[i].Length-1; j++) {
                if (data[i][j] == SEARCHED2[1]) {
                    output += search2(data, i, j);
                }
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
