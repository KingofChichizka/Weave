while (true) 
{
    // Example of how you can use this
    ConsoleColor[] colors = {ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue,
    ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkGray};
    ConsoleColor[] selcolor = new ConsoleColor[5];
    Random r = new Random();
    int n = r.Next(2, 14);
    int complexity = r.Next(2, 6);
    //int n = 16;
    int seed = r.Next();
    Weave w = new Weave(n, complexity , seed);
    int x = 26;
    int y = 80;
    int[,] pattern = w.ReconstructPattern();
    for (int i = 0; i<selcolor.Length; i++) 
    {
        ConsoleColor cc;
        do { cc = colors[r.Next(colors.Length)]; } while(selcolor.Contains(cc));
        selcolor[i] = cc;
    }

    for (int i = 0; i < x; i++)
    {
        for (int j = 0; j < y; j++)
        {
            Console.ForegroundColor = selcolor[pattern[i % ((w.level * 2) - 2), j % ((w.level * 2) - 2)]] ;
            switch (pattern[i % ((w.level * 2) - 2), j % ((w.level * 2) - 2)]) 
            {
                case 0: Console.Write("▓"); break;
                case 1: Console.Write("░"); break;
                case 2: Console.Write("▒"); break;
                case 3: Console.Write("█"); break;
                case 4: Console.Write(" "); break;
            }
        }
        Console.WriteLine();
    }
    Console.Write($"seed = {seed}, level = {w.level}, complexity = {complexity}, Type any button");
    Console.ReadKey();
    Console.Clear();
}

//Class itself
public class Weave 
{
    public int level;
    public int[] pattern;

    //Constructor
    public Weave(int lvl = -1, int complexity = -1, int seed = -1) 
    {
        Random r = new Random();
        if (seed != -1) r = new Random(seed);

        level = lvl;
        if (lvl == -1) lvl = r.Next(2, 16);

        if(complexity == -1) complexity = r.Next(2, 5);

        pattern = new int[Factorial(level)];

        for (int i = 0; i< Factorial(level); i++) 
        {
            pattern[i] = r.Next(complexity);
        }
    }

    //Output of a pattern tile
    public int[,] ReconstructPattern() 
    {
        int[,] tile = new int[(level * 2)-1, (level * 2)-1];
        int s = 0;

        tile[level - 1, level - 1] = pattern[0];
        for (int i = 0; i < ((level * 2) - 1); i++) 
        {
            for (int j = 0; j < ((level * 2) - 1); j++)
            {
                int I = Math.Abs(i- level + 1);
                int J = Math.Abs(j- level + 1);
                if (I > J) { tile[i, j] = pattern[Factorial(I) + J]; }
                else { tile[i, j] = pattern[Factorial(J) + I]; }
            }
        }

        return tile;
    }

    //Calculation of factorial of a number
    int Factorial(int o) 
    {
        int s = 0;
        for (int i = 0; i <= o; i++) { s += i; }
        return s;
    }
}