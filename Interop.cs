using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class Interop
{
    private class Libraries
    {
        internal const string Kernel32 = "kernel32.dll";
        internal const string NtDll = "ntdll.dll";
    }

    internal class NtDll
    {
        internal struct PROCESS_BASIC_INFORMATION
        {
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;
        }

        [DllImport(Libraries.NtDll)]
        internal static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass,
            ref PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);
    }

    internal class Kernel32
    {
        [DllImport(Libraries.Kernel32, SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr handle, out uint mode);

        internal static bool IsGetConsoleModeCallSuccessful(IntPtr handle)
        {
            uint mode;
            return GetConsoleMode(handle, out mode);
        }

        [DllImport(Libraries.Kernel32, SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr handle, uint mode);

        [DllImport(Libraries.Kernel32, SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        internal const uint ENABLE_PROCESSED_INPUT = 0x0001;
        internal const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        internal const int STD_OUTPUT_HANDLE = -11;
    }
}