using System;
using System.Runtime.InteropServices;

internal partial class Interop
{
    internal partial class Libaries
    {
        internal const string Kernel32 = "kernel32.dll";
    }

    internal partial class Kernel32
    {
        [DllImport(Libaries.Kernel32, SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr handle, out uint mode);

        internal static bool IsGetConsoleModeCallSuccessful(IntPtr handle)
        {
            uint mode;
            return GetConsoleMode(handle, out mode);
        }

        [DllImport(Libaries.Kernel32, SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr handle, uint mode);

        [DllImport(Libaries.Kernel32, SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        internal const uint ENABLE_PROCESSED_INPUT = 0x0001;
        internal const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        internal const int STD_OUTPUT_HANDLE = -11;
    }
}