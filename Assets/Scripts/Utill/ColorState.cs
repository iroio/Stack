using UnityEngine;

public static class ColorState
{
    public static bool Initialized = false;

    public static Color cubeColor;
    public static Color bgColor;

    public static int cubeCount;
    public static int bgCount;

    public static void Reset()
    {
        Initialized = false;
    }
}