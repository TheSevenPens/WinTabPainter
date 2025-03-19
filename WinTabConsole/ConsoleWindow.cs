using System;

public static partial class ConsoleWindow
{

    public static void QuickEditMode(bool Enable)
    {
        //QuickEdit lets the user select text in the console window with the mouse, to copy to the windows clipboard.
        //But selecting text stops the console process (e.g. unzipping). This may not be always wanted.
        IntPtr consoleHandle = NativeFunctions.GetStdHandle((int)NativeFunctions.StdHandle.STD_INPUT_HANDLE);
        UInt32 consoleMode;

        NativeFunctions.GetConsoleMode(consoleHandle, out consoleMode);
        if (Enable)
        {
            consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);
        }
        else
        {
            consoleMode &= ~((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);
        }

        consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_EXTENDED_FLAGS);

        NativeFunctions.SetConsoleMode(consoleHandle, consoleMode);
    }
}