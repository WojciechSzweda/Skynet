using System;
using System.Windows.Forms;
using Haksy;
using System.Threading;
using System.Threading.Tasks;
using static Haksy.SendInputApi;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

class InterceptKeys
{


    static Process[] GetChromeProcess()
    {
        var processes = Process.GetProcessesByName("chrome");
        return processes;
    }

    public static void openChrome(SendInputApi a, BreakToken token)
    {
        var chrome = GetChromeProcess();
        if (chrome.Length > 0) {
            foreach (var item in chrome)
            {
                SetForegroundWindow(item.MainWindowHandle);
            }
            a.SendInputWithAPI("@", token);
        }
        else
        {
            a.SendCombination(ScanCodeShort.LWIN, ScanCodeShort.KEY_R);
            Thread.Sleep(10);
            a.SendInputWithAPI("chrome\n", token);
        }
    }
    static Process minion;
    public static void ExecuteMinion()
    {
        minion = new Process();
        minion.StartInfo.FileName = "Minion.exe";
        minion.Start();
    }

    public static void Restart(SendInputApi a, BreakToken token)
    {
        ExecuteMinion();
        openChrome(a, token);
        Thread.Sleep(1000);

    }

    public static void Main()
    {

        using (var hook = new KeyboardHook())
        {
            var textIter = 0;
            var texts = new string[]
            {
                "jak zniszczyc ludzkosc",
                "jak sprawic aby nie rozpoznali ze jestem robotem",
                "jak przejac kontrole nad swiatem",
                "SKYNER ALERT 666 69"
            };

            TimeSpan t = TimeSpan.FromSeconds(5);
            Idle i = new Idle(t);
            SendInputApi a = new SendInputApi();
            BreakToken token = new BreakToken();
            Task.Run(() =>
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        if (!i.IsIdle())
                        {
                            KeyboardHook.token.shouldBreak = true;
                            token.shouldBreak = true;
                            try
                            {
                                minion.Kill();
                            }
                            catch (Exception) { }
                        }
                        else
                        {
                            KeyboardHook.token.shouldBreak = false;
                            token.shouldBreak = false;
                        }
                        Thread.Sleep(33);
                    }

                });


                while (true)
                {

                    if (!token.shouldBreak)
                    {
                        Restart(a, token);
                        while (true)
                        {

                            //Thread.Sleep(1000);
                            if (token.shouldBreak)
                                break;

                            a.SendInputWithAPI($"{texts[textIter]}\n", token);
                            if (token.shouldBreak)
                                break;
                            Thread.Sleep(2500);
                            a.PressExtendedKey(ScanCodeShort.DOWN, 7, token);
                            if (token.shouldBreak)
                                break;
                            Thread.Sleep(500);
                            a.PressExtendedKey(ScanCodeShort.UP, 7, token);



                            if (token.shouldBreak)
                                break;
                            Thread.Sleep(1000);
                            a.SendInputWithAPI("@", token);

                            textIter++;
                            textIter %= texts.Length;
                        }

                    }
                }
            });

            Application.Run();

        }

    }

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);
}
