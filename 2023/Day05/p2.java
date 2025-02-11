import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Collectors;


public class p2 extends Thread {
    public static void main(String[] args) throws FileNotFoundException, InterruptedException {
        List<String> stepsStrings = getFileContent("input.txt");
        List<Double> seeds = getSeeds(stepsStrings.get(0));
        stepsStrings.remove(0);
        List<List<List<Double>>> stepsDouble = formatSteps(stepsStrings);

        List<p2> threads = new ArrayList<>();

        for (int i = 0; i < seeds.size(); i += 2) {
            threads.add(new p2(i, seeds.get(i), seeds.get(i) + seeds.get(i+1), stepsDouble));
        }

        for (p2 thread: threads) {
            thread.start();
        }

        for (p2 thread: threads) {
            thread.join();
        }

        Double location = threads.get(0).location;

        for (p2 thread: threads) {
            if (thread.location < location) {
                location = thread.location;
            }
        }

        System.out.printf("%.0f\n", location);
    }

    public static List<List<List<Double>>> formatSteps(List<String> steps) {
        List<List<List<Double>>> output = new ArrayList<>();

        for (String step: steps) {
            List<List<Double>> outputStep = new ArrayList<>();
            ArrayList<String> stepSplitted = new ArrayList<>(Arrays.asList(step.split("\n")));
            stepSplitted.remove(0);

            for (String range: stepSplitted) {
                outputStep.add(Arrays.asList(range.split(" ")).stream()
                    .map(x -> Double.parseDouble(x))
                    .collect(Collectors.toList()));
            }

            output.add(outputStep);
        }

        return output;
    }

    public static List<Double> getSeeds(String seeds) {
        List<Double> output = new ArrayList<>();

        for (String seed: seeds.split(":")[1].trim().split(" ")) {
            output.add(Double.parseDouble(seed));
        }

        return output;
    }

    public static List<String> getFileContent(String filepath) throws FileNotFoundException {
        File file = new File(filepath);
        Scanner reader = new Scanner(file);
        List<String> output = new ArrayList<>();
        String buffer = "";

        while (reader.hasNextLine()) {
            String line = reader.nextLine();

            if (line.equals("")) {
                output.add(buffer.trim());
                buffer = "";
            } else {
                buffer += line;
                buffer += "\n";
            }
        }

        output.add(buffer.trim());
        reader.close();

        return output;
    }

    private final Integer seed;
    private final Double startSeed, endSeed;
    private final List<List<List<Double>>> stepsDouble;
    public Double location = null;

    public p2(Integer seedNumber, Double start, Double end, List<List<List<Double>>> steps) {
        seed = seedNumber;
        startSeed = start;
        endSeed = end;
        stepsDouble = steps;
    }

    public void run() {
        for (double seed = startSeed; seed < endSeed; seed += 1) {
            if (seed % 10_000_000 == 0) {
                System.out.printf("Thread %d : %.0f/%.0f\n", this.seed, seed, endSeed - 1);
            }

            Double usedSeed = seed;

            for (int j = 0; j < stepsDouble.size(); j++) {
                for (List<Double> range: stepsDouble.get(j)) {
                    Double destinationStart = range.get(0);
                    Double sourceStart = range.get(1);
                    Double length = range.get(2);

                    if (sourceStart <= usedSeed && usedSeed < sourceStart + length) {
                        Double offset = usedSeed - sourceStart;
                        usedSeed = destinationStart + offset;
                        break;
                    }
                }
            }

            if (location == null || usedSeed < location) {
                location = usedSeed;
            }
        }
    }

}
