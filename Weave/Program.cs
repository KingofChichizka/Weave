bool DottingVisibility = true;
bool ColorVisibility = true;
while (true)
{
    // Initialize variables
    ConsoleColor[] selectedColors = new ConsoleColor[5];
    Random random = new Random();
    int patternSize = random.Next(2, 14);
    int colorComplexity = random.Next(2, 6);
    int seed = random.Next();
    Weave weave = new Weave(patternSize, colorComplexity, seed);
    int height = 22;
    int width = 80;
    int[,] pattern = weave.ReconstructPattern();

    // Set console window size
    Console.SetWindowSize(width, height + 2);

    // Generate unique colors
    for (int i = 0; i < selectedColors.Length; i++)
    {
        ConsoleColor color;
        do { color = (ConsoleColor)random.Next(16); } while (selectedColors.Contains(color));
        selectedColors[i] = color;
    }
display:
    // Display pattern
    Console.Clear();
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (ColorVisibility) Console.ForegroundColor = selectedColors[pattern[i % ((weave.size * 2) - 2), j % ((weave.size * 2) - 2)]];
            if (DottingVisibility)
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
    Console.Write($"Seed = {seed}, Size = {weave.size}, Complexity = {colorComplexity}\nPress any ENTER to regenerate, press ESC to leave, press h for controls");
    while (true)
    {
        ConsoleKey pressedKey = Console.ReadKey().Key;
        if (pressedKey == ConsoleKey.Escape) return;
        else if (pressedKey == ConsoleKey.Enter) break;
        else if (pressedKey == ConsoleKey.H)
        {
            Console.Clear();
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"  ____ ___  __  __ __  __    _    _   _ ____  ____  ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @" / ___/ _ \|  \/  |  \/  |  / \  | \ | |  _ \/ ___| ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"| |  | | | | |\/| | |\/| | / _ \ |  \| | | | \___ \ ");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @"| |__| |_| | |  | | |  | |/ ___ \| |\  | |_| |___) |");
            Console.WriteLine(new string(' ', (width - 52) / 2) + @" \____\___/|_|  |_|_|  |_/_/   \_\_| \_|____/|____/ ");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("C:\tToggle color");
            Console.WriteLine("D:\tToggle pattern dotting");
            Console.WriteLine("R:\tResize window");
            //Console.WriteLine("S:\tSave pattern");                                                                              //TODO

            Console.WriteLine("\nPress any key(except power button) to exit this window");
            Console.ReadKey();

            goto display;
        }
        else if (pressedKey == ConsoleKey.C) { ColorVisibility = !ColorVisibility; goto display; }
        else if (pressedKey == ConsoleKey.D) { DottingVisibility = !DottingVisibility; goto display; }
        else if (pressedKey == ConsoleKey.R)
        {
            char selectedProperty = 'w';
            while (true)
            {

                Console.Clear();
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"__        _____ _   _ ____   _____        __  ____ ___ __________ ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"\ \      / /_ _| \ | |  _ \ / _ \ \      / / / ___|_ _|__  / ____|");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @" \ \ /\ / / | ||  \| | | | | | | \ \ /\ / /  \___ \| |  / /|  _|  ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"  \ V  V /  | || |\  | |_| | |_| |\ V  V /    ___) | | / /_| |___ ");
                Console.WriteLine(new string(' ', (width - 66) / 2) + @"   \_/\_/  |___|_| \_|____/ \___/  \_/\_/    |____/___/____|_____|");


                Console.WriteLine(new string('\n', (height / 2) - 3 - 5));

                Console.Write($"{new string(' ', width / 3 - 1)}");
                if(selectedProperty=='w'){Console.BackgroundColor=ConsoleColor.White;Console.ForegroundColor=ConsoleColor.Black;}
                Console.Write($"width");
                Console.ResetColor();
                Console.Write($"{new string(' ', width / 3 - 11)}");
                if(selectedProperty=='h'){Console.BackgroundColor=ConsoleColor.White;Console.ForegroundColor=ConsoleColor.Black;}
                Console.Write($"height");
                Console.ResetColor();
                Console.WriteLine($"{new string(' ', width / 3 - 1)}");

                Console.WriteLine();
                Console.WriteLine($"{new string(' ', width / 3)}  ↑  {new string(' ', (width / 3) - 10)}  ↑  {new string(' ', width / 3)}");
                Console.Write($"{new string(' ', width / 3 - 1)}");
                Console.WriteLine($"  {width} {new string(' ', (width / 3) - 10)}  {height}  ");
                Console.WriteLine($"{new string(' ', width / 3)}  ↓  {new string(' ', (width / 3) - 11)}   ↓  {new string(' ', width / 3)}");

                pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.UpArrow)
                {
                    if (selectedProperty == 'w') width++;
                    else if (selectedProperty == 'h') height++;
                    Console.SetWindowSize(width, height + 2);
                }
                else if (pressedKey == ConsoleKey.DownArrow)
                {
                    if (selectedProperty == 'w') width--;
                    else if (selectedProperty == 'h') height--;
                    Console.SetWindowSize(width, height + 2);
                }
                else if (pressedKey == ConsoleKey.RightArrow) selectedProperty = 'h';
                else if (pressedKey == ConsoleKey.LeftArrow) selectedProperty = 'w';
                else if (pressedKey == ConsoleKey.Escape) break;
            }
            goto display;
        }

    }
    Console.Clear();
}

//Class itself
public class Weave
{
    public int size; // Stores the size of the weave
    public int[] pattern; // Stores the pattern of the weave

    // Constructor for the Weave class
    public Weave(int level = -1, int complexity = -1, int seed = -1)
    {
        Random random = seed != -1 ? new Random(seed) : new Random();

        // Set the size to the provided value or a random value between 2 and 16
        size = level == -1 ? random.Next(2, 16) : level;

        // Set the complexity to the provided value or a random value between 2 and 5
        complexity = complexity == -1 ? random.Next(2, 5) : complexity;

        pattern = new int[CalculatePatternSize()];

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

    // Method to calculate the size of the pattern array based on the size of the weave
    private int CalculatePatternSize()
    {
        return CalculateFactorial(size);
    }
}