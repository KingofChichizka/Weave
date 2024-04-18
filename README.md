# Console Weave Pattern Generator
This C# code generates and displays a weave pattern in the console window.

## Table of Contents
- [Console Weave Pattern Generator](#console-weave-pattern-generator)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Used Libraries](#used-libraries)
  - [Usage](#usage)
  - [Classes](#classes)
    - [Weave](#weave)
    - [Settings](#settings)
    - [Variables](#variables)
    - [Windows](#windows)
    - [AdditionalFunctions](#additionalfunctions)

## Introduction

The program generates a weave pattern using specified settings such as pattern size, color complexity, and seed. It displays the pattern in the console window, allowing the user to toggle color visibility, pattern dotting, and resize the window.

## Used Libraries
- [Spectre.Console](https://spectreconsole.net/)

## Usage

The program generates a weave pattern using specified settings such as pattern size, color complexity, and seed. It displays the pattern in the console window, allowing the user to toggle color visibility, pattern dotting, and resize the window.

- **ENTER:** Regenerates the pattern
- **ESC:** Exits the program
- **D:** Cycle through display modes: Dotted, Colored -> Colored -> Dotted
- **T:** Cycle through pattern type: Square -> Rhomb -> Periodic
- **S:** Save the pattern as an SVG file

## Classes

### Weave

```csharp
public class Weave
{
    public int size;                                    // Stores the size of the weave
    public int[] pattern;                               // Stores the pattern of the weave

    // Constructor for the Weave class
    public Weave(Type type, int size = -1, int complexity = -1, int seed = -1);
    // Method to choose a way of reconstructing an array, depending on a pattern's type
    public int[,] ReconstructPattern();
    // Method to reconstruct the pattern into a 2D array representing the woven square pattern
    private int[,] ReconstructPatternSquare();
    // Method to reconstruct the pattern into a 2D array representing the woven periodic houdstooth-esque pattern
    private int[,] ReconstructPatternPeriodic();
    // Method to reconstruct the pattern into a 2D array representing the woven rhombic pattern
    private int[,] ReconstructPatternRhomb();
    // Method to output 2D array as a string
    public string OutputPatternAsString();
    // Method to calculate the factorial of a number
    private int CalculateFactorial(int number);
    // Method to calculate the diagonal position of a number
    private int CalculateDiagonal(int number);
    // Method to calculate the reverse of a number
    private int CalculateReverse(int number);

}
```

### Settings

```csharp
public class Settings
{
    public static int width;                            // Size of the window
    public static int height;
    public static int patternSize;                      // Size of the pattern
    public static int colorComplexity;                  // Amount of colors
    public static int seed;                             // Seed for the Random
    public static Weave.Type type = 0;                  // Type of the pattern
    public static string[] styleFlagUserNames;          // Array of names displayed in UI 
    public static StyleFlag styleFlag;                  // Flag for styling
    public enum StyleFlag
    {
        Normal,                                         // Display with both color and dots.
        NotDotted,                                      // Display without dots.
        NotColored                                      // Display without color.
    }
}
```
### Variables

```csharp
public class Variables
{
    public static int[,] pattern;                       // The pattern
    public static Weave weave;                          // Weave 
    public static ConsoleColor[] selectedColors;        // Selected colors for this pattern
    public static Random random;                        // Random that is used throughout the code
}
```
### Windows

```csharp
public class Windows
{
    public static bool endOfProcess = false;            // Workaround of aborting the thread
    public static int lastWidth = width;                // Last recorded widht
    public static int lastHeight = height;              // Last recorded height
    public static int msNotResized = 0;                 // How long window wasn't resized
    public static void DisplayPattern();                // Method to display the weave pattern
    public static void DisplayResizeWindow();           // Method to display the resize window
    public static void ResizeListener();                // Listener for window resize events
}
```
### AdditionalFunctions

```csharp
public class AdditionalFunctions
{
    public static void SaveTxt();                        // Method to save pattern as text file
    public static void SaveSvg();                        // Method to save pattern as SVG file
    public static void UpdateWindowSize();               // Updates the size of a pattern window
    public static void LogError(string prefix, Exception exception)  // Logs errors in Markdown
}
```