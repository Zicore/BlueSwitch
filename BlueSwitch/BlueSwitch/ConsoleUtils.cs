using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LabelPrinter
{
    public class ConsoleUtils
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        public static void InitConsole()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
        }

        public static void ShowConsole()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
            Visible = true;
        }

        public static void HideConsole()
        {
            var hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
            Visible = false;
        }

        public static volatile bool Visible = false;
    }
}
