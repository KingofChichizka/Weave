using static Settings;
using static Variables;

while (true)
{
    // Use variables
    selectedColors = new ConsoleColor[5];
    patternSize = random.Next(2, 14);
    colorComplexity = random.Next(2, 6);
    seed = random.Next();
    weave = new Weave(patternSize, colorComplexity, seed);
    pattern = weave.ReconstructPattern();

    // Set console window size
    Console.SetWindowSize(width, height + 2);

    // Generate unique colors
    for (int i = 0; i < selectedColors.Length; i++)
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
        else if (pressedKey == ConsoleKey.H) Windows.DisplayControls();
        else if (pressedKey == ConsoleKey.F) { styleFlag++; if ((int)styleFlag >= 3) styleFlag = 0; }
        else if (pressedKey == ConsoleKey.R) Windows.DisplayWindowSizeSettings();

    }
    Console.Clear();
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
    public static int width = 80;
    public static int height = 22;
    public static StyleFlag styleFlag;
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
    public static void DisplayWindowSizeSettings()
    {
        char selectedProperty = 'w';
        while (true)
        {
            Console.Clear();
            if (width >= 66)
            {
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"__        _____ _   _ ____   _____        __  ____ ___ __________ ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"\ \      / /_ _| \ | |  _ \ / _ \ \      / / / ___|_ _|__  / ____|");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @" \ \ /\ / / | ||  \| | | | | | | \ \ /\ / /  \___ \| |  / /|  _|  ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"  \ V  V /  | || |\  | |_| | |_| |\ V  V /    ___) | | / /_| |___ ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"   \_/\_/  |___|_| \_|____/ \___/  \_/\_/    |____/___/____|_____|");
            }
            else if (width >= 54)
            {
                Console.WriteLine(new string(' ', (width - 54) / 2) + @"          _           _                     _         ");
                Console.WriteLine(new string(' ', (width - 54) / 2) + @"__      _(_)_ __   __| | _____      __  ___(_)_______ ");
                Console.WriteLine(new string(' ', (width - 54) / 2) + @"\ \ /\ / / | '_ \ / _` |/ _ \ \ /\ / / / __| |_  / _ \");
                Console.WriteLine(new string(' ', (width - 54) / 2) + @" \ V  V /| | | | | (_| | (_) \ V  V /  \__ \ |/ /  __/");
                Console.WriteLine(new string(' ', (width - 54) / 2) + @"  \_/\_/ |_|_| |_|\__,_|\___/ \_/\_/   |___/_/___\___|");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string(' ', (width - 11) / 2) + @"WINDOW SIZE");
            }


            Console.WriteLine(new string('\n', (height / 2) - 3 - 5));
            Console.Write($"{new string(' ', width / 3 - 1)}");

            if (selectedProperty == 'w') { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.Write($"width");
            Console.ResetColor();
            Console.Write($"{new string(' ', width / 3 - 11)}");

            if (selectedProperty == 'h') { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.Write($"height");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"{new string(' ', width / 3)}  ↑  {new string(' ', (width / 3) - 10)}  ↑  {new string(' ', width / 3)}");

            Console.Write($"{new string(' ', width / 3 - 1)}");
            Console.WriteLine($"  {width} {new string(' ', (width / 3) - 10)}  {height}  ");

            Console.WriteLine($"{new string(' ', width / 3)}  ↓  {new string(' ', (width / 3) - 11)}   ↓  {new string(' ', width / 3)}");

            Console.WriteLine(new string('\n', (height / 2) - 3));
            Console.Write($"{new string('=', (width - 29) / 2)}Press ESC to exit this window{new string('=', (width - 29) / 2)}");

            ConsoleKey pressedKey = Console.ReadKey().Key;
            if (pressedKey == ConsoleKey.UpArrow)
            {
                if (selectedProperty == 'w') width++;
                else if (selectedProperty == 'h') height++;
                Console.SetWindowSize(width, height + 2);
            }
            else if (pressedKey == ConsoleKey.DownArrow)
            {
                if (selectedProperty == 'w' && width > 33) width--;
                else if (selectedProperty == 'h' && height > 16) height--;
                Console.SetWindowSize(width, height + 2);
            }
            else if (pressedKey == ConsoleKey.RightArrow) selectedProperty = 'h';
            else if (pressedKey == ConsoleKey.LeftArrow) selectedProperty = 'w';
            else if (pressedKey == ConsoleKey.Escape) break;
        }
    }
    public static void DisplayControls()
    {
        Console.Clear();
        if (width >= 52)
        {
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"  ____ ___  __  __ __  __    _    _   _ ____  ____  ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @" / ___/ _ \|  \/  |  \/  |  / \  | \ | |  _ \/ ___| ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"| |  | | | | |\/| | |\/| | / _ \ |  \| | | | \___ \ ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"| |__| |_| | |  | | |  | |/ ___ \| |\  | |_| |___) |");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @" \____\___/|_|  |_|_|  |_/_/   \_\_| \_|____/|____/ ");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(new string(' ', (width - 8) / 2) + "COMMANDS");
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("H:\tShow this help menu");
        Console.WriteLine("F:\tCycle through style flags");
        Console.WriteLine("R:\tResize render window");
        Console.WriteLine("ENTER:\tRegenerates the pattern");
        Console.WriteLine("ESC:\tExits the program");
        //Console.WriteLine("S:\tSave pattern");                                                                   //TODO

        Console.WriteLine("\nPress any key(except power button) to exit this window");
        Console.ReadKey();
    }
    public static void DisplayPattern()
    {
        Console.Clear();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (styleFlag != StyleFlag.NotColored) Console.ForegroundColor = selectedColors[pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)]];
                if (styleFlag != StyleFlag.NotDotted)
                {
                    switch (pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)])
                    {
                        case 0: Console.Write("▓"); break;
                        case 1: Console.Write("░"); break;
                        case 2: Console.Write("▒"); break;
                        case 3: Console.Write("█"); break;
                        case 4: Console.Write(" "); break;
                    }
                }
                else Console.Write("█");
            }
            Console.WriteLine();
        }

        // Prompt user for input
        Console.ResetColor();
        Console.Write($"Seed = {seed}, Size = {weave.size}, Complexity = {colorComplexity}, Style = {styleFlag}\nPress any ENTER to regenerate, press ESC to leave, press h for controls");

    }
}