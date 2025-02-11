class Block {
    public long Id { get; }
    public int Length { get; set; }

    public Block(long id, int length) {
        Id = id;
        Length = length;
    }

    public override string ToString() {
        string output = "";

        for (int i=0; i < Length; i++) {
            if (Id == -1) output += ".";
            else output += Id.ToString();
        }

        return output;
    }
}

class Program {
    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static void part1(string filename) {
        List<long> data = new();

        using(StreamReader reader = File.OpenText(filename)) {
            long id = 0;
            bool isData = true;
            char c;
            int length;

            while (!reader.EndOfStream) {
                c = (char) reader.Read();

                if (c == '\n') break;
                length = int.Parse(c.ToString());

                if (isData) {
                    for (int i=0; i < length; i++) {
                        data.Add(id);
                    }
                    id++;
                } else {
                    for (int i=0; i < length; i++) {
                        data.Add(-1);
                    }
                }

                isData = !isData;
            }
        }

        int ptr1 = 0;
        int ptr2 = data.Count - 1;

        while (true) {
            while (ptr1 != ptr2 && ptr1 < data.Count && ptr2 >= 0 && data[ptr1] != -1) {
                ptr1++;
            }

            while (ptr1 != ptr2 && ptr1 < data.Count && ptr2 >= 0 && data[ptr2] == -1) {
                ptr2--;
            }

            if (ptr1 == ptr2) break;

            data[ptr1] = data[ptr2];
            data[ptr2] = -1;
            ptr2--;
        }

        long output = 0;

        for (int i=0; i < data.Count && data[i] != -1; i++) {
            output += i * data[i];
        }

        Console.WriteLine("part1: " + output);
    }

    static void part2(string filename) {
        List<Block> data = new();
        long id;

        using(StreamReader reader = File.OpenText(filename)) {
            bool isData = true;
            char c;
            int length;
            id = 0;

            while (!reader.EndOfStream) {
                c = (char) reader.Read();

                if (c == '\n') break;
                length = int.Parse(c.ToString());

                if (isData) {
                    data.Add(new(id, length));
                    id++;
                } else {
                    data.Add(new(-1, length));
                }

                isData = !isData;
            }
        }

        int ptr1;
        int ptr2 = data.Count - 1;
        Block block1, block2;

        while (true) {
            while (ptr2 >= 0 && data[ptr2].Id == -1) {
                ptr2--;
            }

            if (ptr2 < 0) break;

            block2 = data[ptr2];
            ptr1 = 0;

            while (ptr1 < ptr2 && (data[ptr1].Id != -1 || data[ptr1].Length < block2.Length)) {
                ptr1++;
            }

            if (ptr1 == ptr2) {
                ptr2--;
                continue;
            }

            block1 = data[ptr1];

            if (block1.Length == block2.Length) {
                data[ptr1] = block2;
                data[ptr2] = block1;
            } else {
                data[ptr1].Length = block1.Length - block2.Length;
                data.RemoveAt(ptr2);
                data.Insert(ptr2, new(-1, block2.Length));
                data.Insert(ptr1, block2);
                ptr2++;
            }
        }

        long output = 0;
        id = 0;

        for (int i=0; i < data.Count; i++) {
            for (int j=0; j < data[i].Length; j++) {
                if (data[i].Id != -1) {
                    output += id * data[i].Id;
                }
                id++;
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
