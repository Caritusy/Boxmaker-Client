using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace   BMReborn
{
    internal class WindowsProxy
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption = "", uint type = 0);

        public static bool MessageBox(string text, string caption = "")
        {
            return MessageBox(IntPtr.Zero, text, caption, 0) == 1;
        }
    }
}
