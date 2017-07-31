using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _700IQ
{
    public class AdvancedCursors
    {
        [DllImport("user32.dll")]
        static extern IntPtr CreateIconFromResource(byte[] presbits, uint dwResSize, bool fIcon, uint dwVer);
        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr LoadCursorFromFile(String str);

        public Cursor SetCursor(string FileName)
        {
            IntPtr hCursor = LoadCursorFromFile(FileName);
            if (!IntPtr.Zero.Equals(hCursor))
                return new Cursor(hCursor);
            else
                return Cursors.Default;
        }

        public static Cursor Create(byte[] resource)
        {
            IntPtr hCursor;

            try
            {
                hCursor = CreateIconFromResource(resource, (uint)resource.Length, false, 0x00030000);
                if (!IntPtr.Zero.Equals(hCursor))
                    return new Cursor(hCursor);
            }
            catch
            {  // resource wrong type or memory error occurred
               // normally this resource exists since you had to put  Properties.Resources. and a resource would appear and you selected it
               // the animate cursor desired  is the error generator since this call is not required for simple cursors
                throw new ApplicationException("Не удалось загрузить курсор из файла ресурсов!");
            }
            return Cursors.Default;
        }
    }
}
