/// W        W   X   W L   W       W       W     R R R  E E E
///  W	    W W   X X  L    W	  W W     A A    R    R E    
///   W    W   W   W   L     W   W   W   W   A   R R R  E E E
///    W  W     W X X  L      W W     W A A A A  R  R   E    
///     W        W   X L L L   W       W       A R   R  E E E
/// https://github.com/WXLWare | https://discord.gg/HDNkFgF2ac
/// Developers: 
/// [Wxlfie]: https://github.com/Wxlfie646 | https://discord.com/users/714978777830129725 

using System.Text;
using System.Runtime.InteropServices;

#region Imports
[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
static extern int GetWindowTextLength(IntPtr hWnd);

[DllImport("user32.dll", SetLastError = true)]
static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

[DllImport("user32.dll")]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool IsWindowVisible(IntPtr hWnd);

[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
static extern bool SetWindowText(IntPtr hwnd, String lpString);

[DllImport("user32.dll", CharSet = CharSet.Auto)]
static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
#endregion

#region Defines
IntPtr HWND;
IntPtr T_HWND;

uint NULL = 0;
uint WM_APPCOMMAND = 0x319;

string window_title = "Spotify Premium";
string window_class = "Chrome_WidgetWin_0";

string target_window_name = ""; // Process you want to rename (optional) window name
string target_window_class = ""; // Process you want to rename window class, obtainable through Winlister.

string current_track = string.Empty;
string current_track_data = string.Empty;

string new_console_title(string input) => $"[@wxlfie] Spotify hook (Debug) | Currently playing -> {input}";
string new_window_title(string input) => $"[@wxlfie]  Spotify hook ({target_window_name}) | Currently playing -> {input}";

bool setcontinue = true;
#endregion

#region Helpers
string GetWinText(IntPtr hWnd)
{
    int length = GetWindowTextLength(hWnd);
    StringBuilder sb = new StringBuilder(length + 1);
    GetWindowText(hWnd, sb, sb.Capacity);
    return sb.ToString();
}
void playpause() { SendMessage(HWND, WM_APPCOMMAND, NULL, 917504); }
void next() { SendMessage(HWND, WM_APPCOMMAND, NULL, 720896); }
void previous() { SendMessage(HWND, WM_APPCOMMAND, NULL, 786432); }
void volumeup() { SendMessage(HWND, WM_APPCOMMAND, NULL, 655360); }
void volumedown() { SendMessage(HWND, WM_APPCOMMAND, NULL, 589824); }
void mute() { SendMessage(HWND, WM_APPCOMMAND, NULL, 524288); }
void stop() { SendMessage(HWND, WM_APPCOMMAND, NULL, 851968); }

#endregion

#region Routines 
void fetch_data()
{
    Console.Out.WriteLine("Fetching data...");
    HWND = FindWindow(window_class, null);
    if (IsWindowVisible(HWND))
    {
        current_track = GetWinText(HWND);
    }
    else Console.Out.WriteLine("Spotify may be minimised");
}
void check_data()
{
    if (current_track == window_title || current_track == "")
        current_track_data = "No song playing/found";
    else current_track_data = current_track;
}
#endregion

#region Methods
void update()
{
    Console.Title = new_console_title(current_track_data);
    T_HWND = FindWindow(target_window_class, null);
    SetWindowText(T_HWND, new_window_title(current_track_data));
}

void main()
{
    while (setcontinue)
    {
        fetch_data();
        check_data();

        update();
        System.Threading.Thread.Sleep(1700);
    }
}
#endregion

main();
