using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Pillsgood.Extensions.Logging
{
    internal static class ProcessExtensions
    {
        public static Process Parent(this Process process) => GetParentProcess(process.Handle);

        private static Process GetParentProcess(IntPtr handle)
        {
            var processBasicInformation = new Interop.NtDll.PROCESS_BASIC_INFORMATION();
            var status = Interop.NtDll.NtQueryInformationProcess(handle, 0, ref processBasicInformation,
                Marshal.SizeOf(processBasicInformation), out _);
            if (status != 0)
            {
                throw new Win32Exception(status);
            }

            try
            {
                return Process.GetProcessById(processBasicInformation.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}