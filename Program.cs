using System.Text;
using Spectre.Console;
using static Settings;
using static Variables;
using static AdditionalFunctions;

while (true)
{
    // Use variables
    selectedColors = new ConsoleColor[15];
    patternSize = random.Next(2, 14);
    colorComplexity = random.Next(2, 15);
    seed = random.Next();
    weave = new Weave(patternSize, colorComplexity, seed);
    pattern = weave.ReconstructPattern();

    // Generate unique colors
    for (int i = 0; i < colorComplexity; i++)
    {
        ConsoleColor color;
        do { color = (ConsoleColor)random.Next(16); } while (selectedColors.Contains(color));
        selectedColors[i] = color;
    }
    // Handle user input
    while (true)
    {
        Windows.DisplayPattern();

        ConsoleKey pressedKey = Console.ReadKey().Key;
        if (pressedKey == ConsoleKey.Escape) return;
        else if (pressedKey == ConsoleKey.Enter) break;
        else if (pressedKey == ConsoleKey.D) { styleFlag++; if ((int)styleFlag >= 3) styleFlag = 0; }
        // else if (pressedKey == ConsoleKey.R) DisplayWindowSizeSettings();                                                //TODO resizing window
        else if (pressedKey == ConsoleKey.S) SaveSvg();                                                                     //TODO saving window 
    }
    AnsiConsole.Clear();
}
public class Weave
{
    public int size; // Stores the size of the weave
    public int[] pattern; // Stores the pattern of the weave

    // Constructor for the Weave class
    public Weave(int size = -1, int complexity = -1, int seed = -1)
    {
        Random random = seed != -1 ? new Random(seed) : new Random();

        // Set the size to the provided value or a random value between 2 and 16
        this.size = size == -1 ? random.Next(2, 16) : size;

        // Set the complexity to the provided value or a random value between 2 and 5
        complexity = complexity == -1 ? random.Next(2, 5) : complexity;

        pattern = new int[CalculateFactorial(size)];

        // Assign random values to each element in the pattern array
        for (int i = 0; i < pattern.Length; i++)
        {
            pattern[i] = random.Next(complexity);
        }
    }

    // Method to reconstruct the pattern into a 2D array representing the woven pattern
    public int[,] ReconstructPattern()
    {
        int[,] tile = new int[(size * 2) - 1, (size * 2) - 1];

        // Set the central tile of the pattern array in the 2D tile array
        tile[size - 1, size - 1] = pattern[0];

        // Loop through each cell of the 2D tile array
        for (int i = 0; i < ((size * 2) - 1); i++)
        {
            for (int j = 0; j < ((size * 2) - 1); j++)
            {
                int I = Math.Abs(i - size + 1);
                int J = Math.Abs(j - size + 1);
                if (I > J)
                {
                    tile[i, j] = pattern[CalculateFactorial(I) + J];
                }
                else
                {
                    tile[i, j] = pattern[CalculateFactorial(J) + I];
                }
            }
        }

        return tile;
    }
    // Method to output 2D array as a string
    public string OutputPatternAsString()
    {
        string s = "";
        foreach (int i in pattern)
        {
            s += i + " ";
        }
        return s;
    }

    // Method to calculate the factorial of a number
    private int CalculateFactorial(int number)
    {
        int sum = 0;
        for (int i = 0; i <= number; i++)
        {
            sum += i;
        }
        return sum;
    }

}
public class Settings
{
    public static int width = Console.WindowWidth * 5 / 6 - 4;
    public static int height = Console.WindowHeight - 2;
    public static StyleFlag styleFlag;
    public static string[] styleFlagUserNames = ["Dotted, Colored", "Colored", "Dotted"];
    public static int patternSize = -1;
    public static int colorComplexity = -1;
    public static int seed = -1;
    public enum StyleFlag
    {
        Normal,
        NotDotted,
        NotColored
    }
}
public class Variables
{
    public static int[,] pattern = new int[0, 0];
    public static Weave weave = null;
    public static ConsoleColor[] selectedColors = Array.Empty<ConsoleColor>();
    public static Random random = new Random();
}
public class Windows
{
    public static void DisplayPattern()
    {
        AnsiConsole.Clear();
        UpdateWindowSize();
        // Create pattern string
        string patterntemplate = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (styleFlag != StyleFlag.NotColored) patterntemplate += $"[{(int)selectedColors[pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)]]}]";
                if (styleFlag != StyleFlag.NotDotted)
                {
                    switch (pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)] % 4)
                    {
                        case 0: patterntemplate += "▓"; break;
                        case 1: patterntemplate += "░"; break;
                        case 2: patterntemplate += "▒"; break;
                        case 3: patterntemplate += "█"; break;
                    }
                }
                else patterntemplate += "█";
                if (styleFlag != StyleFlag.NotColored) patterntemplate += "[/]";
            }
        }
        // Display
        // Create the layout
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("pattern"),
                new Layout("info").SplitRows(
                    new Layout("properties"),
                    new Layout("controls")
                ).MinimumSize(20));

        // Update the pattern column
        layout["pattern"].Update(
            new Panel(
                Align.Center(
                    new Markup($"{patterntemplate}"),
                    VerticalAlignment.Middle))
                .RoundedBorder().Expand().Header(" Pattern "));
        layout["pattern"].Ratio(5);
        layout["info"].Ratio(1);
        layout["info"]["properties"].Update(new Panel(new Markup($"[red]Seed[/]: {seed}\n[red]Complexity[/]: {colorComplexity}\n[red]Size[/]: {patternSize}")).RoundedBorder().Header(" Info ").Expand());
        layout["info"]["properties"].Ratio(1);
        layout["info"]["controls"].Update(new Panel(new Markup($"[red]D[/]: Display mode: {styleFlagUserNames[(int)styleFlag]}\n\n[red]S[/]: Save pattern")).RoundedBorder().Header(" Controls "));
        layout["info"]["controls"].Ratio(2);
        AnsiConsole.Write(layout);
    }
}
public class AdditionalFunctions
{
    public static void SaveTxt()
    {
        Directory.CreateDirectory("Saved");
        using (FileStream fs = File.Create($@"Saved\w_{seed}_{patternSize}_{colorComplexity}.txt"))
        {
            Byte[] title = new UTF8Encoding(true).GetBytes($"{seed} {patternSize} {colorComplexity} {weave.OutputPatternAsString()}");
            fs.Write(title, 0, title.Length);
        }
    }
    public static void SaveSvg()                                                                                                    //FIXME 
    {
        string[] fills ={
            "#000000", // black
            "#800000", // maroon
            "#008000", // green
            "#808000", // olive
            "#000080", // navy
            "#800080", // purple
            "#008080", // teal
            "#c0c0c0", // silver
            "#808080", // grey
            "#ff0000", // red
            "#00ff00", // lime
            "#ffff00", // yellow
            "#0000ff", // blue
            "#ff00ff", // fuchsia
            "#00ffff", // aqua
            "#ffffff"  // white
        };
        Directory.CreateDirectory("Saved");
        File.Create($"./Saved/w_{seed}_{patternSize}_{colorComplexity}.svg").Close();
        using (StreamWriter fileWriter = new StreamWriter($@"./Saved/w_{seed}_{patternSize}_{colorComplexity}.svg"))
        {
            fileWriter.WriteLine($"<svg viewBox='0 0 {width * 10} {height * 16}' xmlns='http://www.w3.org/2000/svg'>");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    fileWriter.WriteLine($"<rect width='11' height='17' x='{j * 10}' y='{i * 16}' fill='{fills[(int)selectedColors[pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)]]]}'/>");
                }
            }

            fileWriter.WriteLine("</svg>");
        }

    }
    public static void UpdateWindowSize()
    {
        width = Console.WindowWidth * 5 / 6 - 4;
        height = Console.WindowHeight - 2;
    }
}