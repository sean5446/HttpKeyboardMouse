using System.Runtime.InteropServices;
using Application = System.Windows.Forms.Application;

namespace HttpKeyboardMouse
{
    internal class WinAPI
    {
        const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */
        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;

        public const int VK_VOLUME_MUTE = 0xAD;	        // Volume Mute key
        public const int VK_VOLUME_DOWN = 0xAE;	        // Volume Down key
        public const int VK_VOLUME_UP = 0xAF;	        // Volume Up key
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;    // Next Track key
        public const int VK_MEDIA_PREV_TRACK = 0xB1;    // Previous Track key
        public const int VK_MEDIA_STOP = 0xB2;	        // Stop Media key
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;    // Play/Pause Media key

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;

        public static int Delay = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }



        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        static extern int GetSystemMetrics(int index);



        public static void MouseLeftClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public static void MouseRightClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
        }

        public static void MouseMiddleClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_MIDDLEUP, xpos, ypos, 0, 0);
        }

        private static void DoEvent(int eventType, int globalX, int globalY)
        {
            globalX = globalX > 0 ? globalX : 0;
            globalY = globalY > 0 ? globalY : 0;
            int x = Convert.ToInt32((float)globalX * 65536 / GetSystemMetrics(SM_CXSCREEN));
            int y = Convert.ToInt32((float)globalY * 65536 / GetSystemMetrics(SM_CYSCREEN));
            mouse_event(MOUSEEVENTF_ABSOLUTE | eventType, x, y, 0, 0);
            Thread.Sleep(Delay);
            Application.DoEvents();
        }

        public static void MouseLeftDown(int globalX, int globalY)
        {
            DoEvent(MOUSEEVENTF_MOVE, globalX, globalY);
            DoEvent(MOUSEEVENTF_LEFTDOWN, globalX, globalY);
        }

        public static void MouseLeftUp(int globalX, int globalY)
        {
            DoEvent(MOUSEEVENTF_MOVE, globalX, globalY);
            DoEvent(MOUSEEVENTF_LEFTUP, globalX, globalY);
        }

        public static void MouseRightDown(int globalX, int globalY)
        {
            DoEvent(MOUSEEVENTF_MOVE, globalX, globalY);
            DoEvent(MOUSEEVENTF_RIGHTDOWN, globalX, globalY);
        }

        public static void MouseRightUp(int globalX, int globalY)
        {
            DoEvent(MOUSEEVENTF_MOVE, globalX, globalY);
            DoEvent(MOUSEEVENTF_RIGHTUP, globalX, globalY);
        }

        public static void MoveCursorTo(int globalX, int globalY, int speed)
        {
            if (speed != 0)
            {
                int cursorX = Cursor.Position.X;
                int cursorY = Cursor.Position.Y;
                int dX = globalX - cursorX;
                int dY = globalY - cursorY;
                int steps = Math.Max(Math.Abs(dX / speed), Math.Abs(dY / speed));
                if (steps != 0)
                {
                    int stepX = dX / steps;
                    int stepY = dY / steps;
                    for (int i = 0; i < steps; i++)
                    {
                        cursorX += stepX;
                        cursorY += stepY;
                        DoEvent(MOUSEEVENTF_MOVE, cursorX, cursorY);
                    }
                }
            }
            DoEvent(MOUSEEVENTF_MOVE, globalX, globalY);
            Thread.Sleep(Delay);
            Application.DoEvents();
        }

        public static void MoveCursorTo(int globalX, int globalY)
        {
            MoveCursorTo(globalX, globalY, 0);
        }

        public static void MouseDragAndDrop(int startX, int startY, int finishX, int finishY)
        {
            MouseLeftDown(startX, startY);
            MoveCursorTo(finishX, finishY, 10);
            MouseLeftUp(finishX, finishY);
        }

        public static void SendKey(int key)
        {
            keybd_event((byte)key, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static int GetNumberOfScreens()
        {
            return Screen.AllScreens.Length;
        }

        public static void SetCursorInMiddleOfScreen(int screenNumber)
        {
            if (screenNumber < 0 || screenNumber > Screen.AllScreens.Length - 1) 
                return; 

            var screens = Screen.AllScreens;
            int x = screens[screenNumber].WorkingArea.Width / 2 + screens[screenNumber].WorkingArea.X;
            int y = screens[screenNumber].WorkingArea.Height / 2 + screens[screenNumber].WorkingArea.Y;

            SetCursorPos(x, y);
        }

    }
}
