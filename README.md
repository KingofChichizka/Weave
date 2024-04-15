# Console Weave Pattern Generator
This C# code generates and displays a weave pattern in the console window.

## Table of Contents
- [Console Weave Pattern Generator](#console-weave-pattern-generator)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Usage](#usage)
  - [Classes](#classes)
    - [Weave](#weave)
    - [Settings](#settings)
    - [Variables](#variables)
    - [Windows](#windows)

## Introduction

The program generates a weave pattern using specified settings such as pattern size, color complexity, and seed. It displays the pattern in the console window, allowing the user to toggle color visibility, pattern dotting, and resize the window.

## Usage

The program runs in an infinite loop until the user exits by pressing the ESC key. During each iteration, it generates a new weave pattern based on the provided or random settings. The user can interact with the program by pressing keys:
- **ENTER:** Regenerates the pattern
- **ESC:** Exits the program
- **H:** Displays controls
- **R:** Displays window size settings
- **F** Cycle through style flags

## Classes

### Weave

```csharp
public class Weave
{
    public int size;                        // Stores the size of the weave
    public int[] pattern;                   // Stores the pattern of the weave

    // Constructor for the Weave class
    public Weave(int size = -1, int complexity = -1, int seed = -1)

    // Method to reconstruct the pattern into a 2D array representing the woven pattern
    public int[,] ReconstructPattern()

    // Method to calculate the factorial of a number
    private int CalculateFactorial(int number)
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
    public static StyleFlag styleFlag;                  // Flag for styling
    public enum StyleFlag
    {
        Normal,
        NotDotted,
        NotColored
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
    public static Random random;                        // Random that is used through out the code
}
```
### Windows

```csharp
public class Windows
{
    public static void DisplayWindowSizeSettings()      // Method to display window size settings
    public static void DisplayControls()                // Method to display controls
    public static void DisplayPattern()                 // Method to display the weave pattern
}
```