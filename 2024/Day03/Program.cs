class Program {
    static readonly string CHARS = "mult(,)0123456789";
    static readonly string CHARS_CONDITIONAL = "don'mult(,)0123456789";

    static void Main(string[] args) {
        if (args.Length != 1) { return; }

        part1(args[0]);
        part2(args[0]);
    }

    static bool readString(in StreamReader reader, in string delimiter, in string charset) {
        for (int i=0; i < delimiter.Length; i++) {
            char next = (char) reader.Peek();

            if (!charset.Contains(next)) {
                reader.Read();
                return false;
            }

            if (next != delimiter[i]) {
                reader.Read();
                return false;
            }

            reader.Read();
        }

        return true;
    }

    static bool readInteger(in StreamReader reader, in string charset, out int number) {
        int output = 0;
        int count = 0;

        while (true) {
            char next = (char) reader.Peek();

            if (!charset.Contains(next)) {
                reader.Read();
                number = 0;
                return false;
            }

            if (next < '0' || '9' < next) {
                break;
            }

            output = output * 10 + (next - 0x30);
            count++;
            reader.Read();
        }

        if (count < 1 || 3 < count) {
            number = 0;
            return false;
        }

        number = output;
        return true;
    }

    static bool parser(in StreamReader reader, out int[] numbers) {
        int number1, number2;

        if (!readString(reader, "mul(", CHARS)) {
            numbers = [];
            return false;
        }

        if (!readInteger(reader, CHARS, out number1)) {
            numbers = [];
            return false;
        }

        if (!readString(reader, ",", CHARS)) {
            numbers = [];
            return false;
        }

        if (!readInteger(reader, CHARS, out number2)) {
            numbers = [];
            return false;
        }

        if (!readString(reader, ")", CHARS)) {
            numbers = [];
            return false;
        }

        numbers = new int[]{number1, number2};
        return true;
    }

    static void part1(string filename) {
        int output = 0;

        using(StreamReader reader = File.OpenText(filename)) {
            while (!reader.EndOfStream) {
                int[] numbers = new int[]{};
                bool parsing = parser(reader, out numbers);

                if (parsing) {
                    output += numbers[0] * numbers[1];
                }
            }
        }

        Console.WriteLine("part1: " + output);
    }

    static bool readStringConditional(in string input, in string delimiter, in string charset, in int index, out int length) {
        /* Console.WriteLine("searching " + delimiter); */
        for (int i=0; i < delimiter.Length && index+i < input.Length; i++) {
            char next = input[index+i];

            if (!charset.Contains(next)) {
                length = i;
                return false;
            }

            if (next != delimiter[i]) {
                length = 0;
                return false;
            }
        }

        length = delimiter.Length;
        return true;
    }

    static bool readIntegerConditional(in string input, in string charset, in int index, out int length, out int number) {
        /* Console.WriteLine("searching integer"); */
        int output = 0;
        int count = 0;

        while (true) {
            char next = input[index+count];

            if (!charset.Contains(next)) {
                number = 0;
                length = 0;
                return false;
            }

            if (next < '0' || '9' < next) {
                break;
            }

            output = output * 10 + (next - 0x30);
            count++;
        }

        if (count < 1 || 3 < count) {
            number = 0;
            length = 0;
            return false;
        }

        number = output;
        length = count;
        return true;
    }

    static bool parserConditional(in string input, out int[] numbers, ref int index, ref bool state) {
        int number1, number2;
        int length;
        /* Console.WriteLine(input[index]); */

        while (index < input.Length && !CHARS_CONDITIONAL.Contains(input[index])) {
            index++;
        }

        if (readStringConditional(input, "do()", CHARS_CONDITIONAL, index, out length)) {
            numbers = [];
            index += length;
            state = true;
            /* Console.WriteLine("do"); */
            return false;
        }

        if (readStringConditional(input, "don't()", CHARS_CONDITIONAL, index, out length)) {
            numbers = [];
            index += length;
            state = false;
            /* Console.WriteLine("don't"); */
            return false;
        }

        if (!readStringConditional(input, "mul(", CHARS_CONDITIONAL, index, out length)) {
            numbers = [];
            return false;
        }

        index += length;

        if (!readIntegerConditional(input, CHARS_CONDITIONAL, index, out length, out number1)) {
            numbers = [];
            return false;
        }

        index += length;

        if (!readStringConditional(input, ",", CHARS_CONDITIONAL, index, out length)) {
            numbers = [];
            return false;
        }

        index += length;

        if (!readIntegerConditional(input, CHARS_CONDITIONAL, index, out length, out number2)) {
            numbers = [];
            return false;
        }

        index += length;

        if (!readStringConditional(input, ")", CHARS_CONDITIONAL, index, out length)) {
            numbers = [];
            return false;
        }

        index += length;

        numbers = new int[]{number1, number2};
        return true;
    }

    static void part2(string filename) {
        int output = 0;
        string input = "";

        using(StreamReader reader = File.OpenText(filename)) {
            while (!reader.EndOfStream) {
                input += reader.ReadLine();
            }
        }

        bool state = true;
        int index = 0;

        while (index < input.Length) {
            int[] numbers = new int[]{};
            bool parsing = parserConditional(input, out numbers, ref index, ref state);

            if (parsing) {
                if (state) {
                    /* Console.WriteLine($"{numbers[0]} * {numbers[1]}"); */
                    output += numbers[0] * numbers[1];
                }
            } else {
                index++;
            }
        }

        Console.WriteLine("part2: " + output);
    }
}
