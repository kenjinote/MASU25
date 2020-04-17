using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MASU25
{
    class WinAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

        public const int GWL_HINSTANCE = (-6);
        public const int WH_CBT = 5;
        public const int HCBT_ACTIVATE = 5;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;

        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
    /// <summary>
    /// 拡張メッセージボックス
    /// </summary>
    class CustomMsgBox
    {
        /// <summary>
        /// 親ウィンドウ
        /// </summary>
        private Window ownerWindow = null;

        /// <summary>
        /// フックハンドル
        /// </summary>
        private IntPtr hHook = IntPtr.Zero;

        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(
            Window owner,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {

            if (owner.WindowState == WindowState.Minimized)
            {
                return MessageBox.Show(owner, messageBoxText, caption, button, icon);
            }
            else
            {
                CustomMsgBox mbox = new CustomMsgBox(owner);
                return mbox.Show(messageBoxText, caption, button, icon);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="window">Owner Window</param>
        private CustomMsgBox(Window window)
        {
            ownerWindow = window;
        }

        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        private MessageBoxResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            // フックを設定する。
            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(ownerWindow);
            IntPtr hInstance = WinAPI.GetWindowLong(hwndSource.Handle, WinAPI.GWL_HINSTANCE);
            IntPtr threadId = WinAPI.GetCurrentThreadId();
            hHook = WinAPI.SetWindowsHookEx(WinAPI.WH_CBT, new WinAPI.HOOKPROC(HookProc), hInstance, threadId);

            return MessageBox.Show(ownerWindow, messageBoxText, caption, button, icon);
        }

        /// <summary>
        /// フックプロシージャ
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode == WinAPI.HCBT_ACTIVATE)
            {
                WinAPI.RECT rcForm = new WinAPI.RECT(0, 0, 0, 0);
                WinAPI.RECT rcMsgBox = new WinAPI.RECT(0, 0, 0, 0);

                HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(ownerWindow);
                WinAPI.GetWindowRect(hwndSource.Handle, out rcForm);
                WinAPI.GetWindowRect(wParam, out rcMsgBox);

                // センター位置を計算する。
                int x = (rcForm.Left + (rcForm.Right - rcForm.Left) / 2) - ((rcMsgBox.Right - rcMsgBox.Left) / 2);
                int y = (rcForm.Top + (rcForm.Bottom - rcForm.Top) / 2) - ((rcMsgBox.Bottom - rcMsgBox.Top) / 2);

                WinAPI.SetWindowPos(wParam, 0, x, y, 0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER | WinAPI.SWP_NOACTIVATE);

                IntPtr result = WinAPI.CallNextHookEx(hHook, nCode, wParam, lParam);

                // フックを解除する。
                WinAPI.UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;

                return result;

            }
            else
            {
                return WinAPI.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
    }
}
