using System.Text;
using Spectre.Console;
using static Settings;
using static Variables;
using static AdditionalFunctions;

while (true)
{
    Thread sizeWhatchThread = new Thread(new ThreadStart(Windows.ResizeListener));
    sizeWhatchThread.Start();
    // Use variables
    selectedColors = new ConsoleColor[15];
    patternSize = random.Next(2, 14);
    colorComplexity = random.Next(2, 15);
    seed = random.Next();
    weave = new Weave(type, patternSize, colorComplexity, seed);
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
        try
        {
            Windows.DisplayPattern();

            ConsoleKey pressedKey = Console.ReadKey().Key;
            if (pressedKey == ConsoleKey.Escape) { Windows.endOfProcess = true; return; }
            else if (pressedKey == ConsoleKey.Enter) break;
            else if (pressedKey == ConsoleKey.T) { type++; if ((int)type >= 3) type = 0; }
            else if (pressedKey == ConsoleKey.D) { styleFlag++; if ((int)styleFlag >= 3) styleFlag = 0; }
            // else if (pressedKey == ConsoleKey.R) DisplayWindowSizeSettings();                                                //TODO resizing window
            else if (pressedKey == ConsoleKey.S) SaveSvg();                                                                     //TODO saving window 
        }
        catch (Exception e)
        {
            LogError("MAIN", e);
        }

    }
    AnsiConsole.Clear();
}
public class Weave
{
    public int size; // Stores the size of the weave
    public int[] pattern; // Stores the pattern of the weave
    public Type type; //Stores the type of the weave
    public enum Type
    {
        Square,
        Rhomb,
        Periodic
    };

    // Constructor for the Weave class
    public Weave(Type type, int size = -1, int complexity = -1, int seed = -1)
    {
        Random random = seed != -1 ? new Random(seed) : new Random();

        // Set the size to the provided value or a random value between 2 and 16
        this.size = size == -1 ? random.Next(2, 16) : size;

        // Set the complexity to the provided value or a random value between 2 and 5
        if (type != Type.Periodic) complexity = complexity == -1 ? random.Next(2, 5) : complexity;
        else complexity = 2;

        this.type = type;

        switch (type)
        {
            case Type.Square: { pattern = new int[CalculateFactorial(size)]; } break;
            case Type.Rhomb: { pattern = new int[CalculateDiagonal(size)]; } break;
            case Type.Periodic: { pattern = new int[size * size]; } break;
        }

        // Assign random values to each element in the pattern array
        for (int i = 0; i < pattern.Length; i++)
        {
            pattern[i] = random.Next(complexity);
        }
    }
    // Method to choose a way of reconstructing an array, depending on a pattern's type
    public int[,] ReconstructPattern()
    {
        switch (type)
        {
            case Type.Square: { return ReconstructPatternSquare(); }
            case Type.Rhomb: { return ReconstructPatternRhomb(); }
            case Type.Periodic: { return ReconstructPatternPeriodic(); }
            default: { return null; }
        }
    }
    // Method to reconstruct the pattern into a 2D array representing the woven square pattern
    private int[,] ReconstructPatternSquare()
    {
        int[,] tile = new int[(size * 2) - 2, (size * 2) - 2];

        // Set the central tile of the pattern array in the 2D tile array
        tile[size - 1, size - 1] = pattern[0];

        // Loop through each cell of the 2D tile array
        for (int i = 0; i < ((size * 2) - 2); i++)
        {
            for (int j = 0; j < ((size * 2) - 2); j++)
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
    // Method to reconstruct the pattern into a 2D array representing the woven periodic houdstooth-esque pattern
    private int[,] ReconstructPatternPeriodic()
    {
        int[,] tile = new int[(size * 2), (size * 2)];
        // Loop through each cell of the 2D tile array
        for (int i = 0; i < size * 2; i++)
        {
            for (int j = 0; j < size * 2; j++)
            {
                int I = Math.Abs(i % size);
                int J = Math.Abs(j % size);
                if ((i < size && j < size) || (i >= size && j >= size))
                {
                    tile[i, j] = pattern[J + (I * size)];
                }
                else
                {
                    tile[i, j] = CalculateReverse(pattern[J + (I * size)]);
                }
            }
        }
        return tile;
    }
    // Method to reconstruct the pattern into a 2D array representing the woven rhombic pattern
    private int[,] ReconstructPatternRhomb()
    {
        int[,] tile = new int[(size * 2) - 2, (size * 2) - 2];

        // Set the central tile of the pattern array in the 2D tile array
        tile[size - 1, size - 1] = pattern[0];

        // Loop through each cell of the 2D tile array
        for (int i = 0; i < ((size * 2) - 2); i++)
        {
            for (int j = 0; j < ((size * 2) - 2); j++)
            {
                int I = size - 1 - Math.Abs(i - size + 1);
                int II = Math.Abs(i - size + 1);
                int J = size - 1 - Math.Abs(j - size + 1);
                int JJ = Math.Abs(j - size + 1);
                if (I >= J)
                {
                    if (CalculateDiagonal(I + J) + J <= CalculateDiagonal(size) - 1) tile[i, j] = pattern[CalculateDiagonal(I + J) + J];
                    else tile[i, j] = pattern[CalculateDiagonal(II + JJ) + II];
                }
                else
                {
                    if (CalculateDiagonal(I + J) + I <= CalculateDiagonal(size) - 1) tile[i, j] = pattern[CalculateDiagonal(I + J) + I];
                    else tile[i, j] = pattern[CalculateDiagonal(II + JJ) + JJ];
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
    // Method to calculate the diagonal position of a number
    private int CalculateDiagonal(int number)
    {
        if (number > 0)
        {
            int sum = 1;
            for (int i = 1; i < number; i++)
            {
                sum += (i / 2) + 1;
            }
            return sum;
        }
        else return 0;
    }
    // Method to calculate the reverse of a number
    private int CalculateReverse(int number)
    {
        if (number == 0) return 1;
        else return 0;
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
    public static Weave.Type type = 0;
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
    public static bool endOfProcess = false; // Workaround of aborting the thread
    public static int lastWidth = width; // Last recorded widht
    public static int lastHeight = height; // Last recorded height
    public static int msNotResized = 0; // How long window wasn't resized
    public static WindowType currentWindow; // Current type of window
    public enum WindowType
    {
        PatternWindow,
        ResizeWindow,
    }

    public static void DisplayPattern()
    {
        currentWindow = WindowType.PatternWindow;
        AnsiConsole.Clear();
        UpdateWindowSize();
        // Create pattern string
        string patterntemplate = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (styleFlag != StyleFlag.NotColored) patterntemplate += $"[{(int)selectedColors[pattern[i % ((int)Math.Sqrt(pattern.Length)), j % ((int)Math.Sqrt(pattern.Length))]]}]";
                if (styleFlag != StyleFlag.NotDotted)
                {
                    switch (pattern[i % ((int)Math.Sqrt(pattern.Length)), j % ((int)Math.Sqrt(pattern.Length))] % 4)
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
            patterntemplate += "\n";
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
        layout["info"]["controls"].Update(new Panel(new Markup($"[red]ESC[/]: Finish\n[red]ENTER[/]: Generate new pattern\n[red]T[/]: Pattern type: [darkKhaki]{type.ToString()}[/]\n[red]D[/]: Display mode: [darkKhaki]{styleFlagUserNames[(int)styleFlag]}[/]\n\n[red]S[/]: Save pattern")).RoundedBorder().Header(" Controls "));
        layout["info"]["controls"].Ratio(2);
        AnsiConsole.Write(layout);
    }
    public static void DisplayResizeWindow()
    {
        currentWindow = WindowType.ResizeWindow;
        AnsiConsole.Clear();
        AnsiConsole.Write(new Layout("Root").Update(new Panel(Align.Center(new Markup($"{Console.WindowWidth}x{Console.WindowHeight}"), VerticalAlignment.Middle)).RoundedBorder().Expand()));
    }
    public static void ResizeListener()
    {
        while (!endOfProcess)
        {

            UpdateWindowSize();
            if (width != lastWidth || height != lastHeight)
            {
                DisplayResizeWindow();
                lastWidth = width;
                lastHeight = height;
                msNotResized = 0;
            }
            else msNotResized += 25;
            Thread.Sleep(25);
            if (msNotResized == 2000)
            {
                if(currentWindow == WindowType.ResizeWindow) DisplayPattern();
            }
        }
    }
}
public class AdditionalFunctions
{
    // Saves weave as a txt file
    public static void SaveTxt()
    {
        Directory.CreateDirectory("Saved");
        using (FileStream fs = File.Create($@"Saved\{type.ToString()}_{seed}_{patternSize}_{colorComplexity}.txt"))
        {
            Byte[] title = new UTF8Encoding(true).GetBytes($"{(int)type} {seed} {patternSize} {colorComplexity} {weave.OutputPatternAsString()}");
            fs.Write(title, 0, title.Length);
        }
    }
    // Saves weave as an svg file
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
        File.Create($"./Saved/{type.ToString()}_{seed}_{patternSize}_{colorComplexity}.svg").Close();
        using (StreamWriter fileWriter = new StreamWriter($@"./Saved/{type.ToString()}_{seed}_{patternSize}_{colorComplexity}.svg"))
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
    // Updates the size of a pattern window
    public static void UpdateWindowSize()
    {
        width = (Console.WindowWidth > 120) ? Console.WindowWidth * 5 / 6 - 4 : Console.WindowWidth - 20 - 4;
        height = Console.WindowHeight - 2;
    }
    // Logs errors in Markdown
    public static void LogError(string prefix, Exception exception)
    {
        File.AppendAllText("errors.md", $"***\n`[{prefix}]` **{exception.GetType()}** at {DateTime.Now}:\n> {exception.Message}\n\n");
    }
}