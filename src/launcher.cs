// Pipeline Design launcher: extracts the embedded HTML to the user data folder and opens it
// in its own window with Edge (or Chrome), without an address bar. No internet needed.
// Boru Hattı Tasarımı başlatıcısı: gömülü HTML dosyasını çıkarır ve kendi penceresinde açar.
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

// © 2026 Yiğit Berke Arıkan — MIT License
[assembly: AssemblyTitle("Pipeline Design")]
[assembly: AssemblyProduct("Pipeline Design (Boru Hattı Tasarımı)")]
[assembly: AssemblyCompany("Yiğit Berke Arıkan")]
[assembly: AssemblyCopyright("© 2026 Yiğit Berke Arıkan")]
[assembly: AssemblyVersion("1.0.0.0")]

static class Launcher
{
    static string FindBrowser()
    {
        string[] keys = { @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\msedge.exe", @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe" };
        foreach (string k in keys)
            foreach (RegistryKey root in new[] { Registry.CurrentUser, Registry.LocalMachine })
                using (RegistryKey rk = root.OpenSubKey(k))
                {
                    string p = rk == null ? null : rk.GetValue(null) as string;
                    if (!string.IsNullOrEmpty(p) && File.Exists(p)) return p;
                }
        string[] fixedPaths = {
            Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe"),
            Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Microsoft\Edge\Application\msedge.exe"),
            Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Google\Chrome\Application\chrome.exe")
        };
        foreach (string p in fixedPaths) if (File.Exists(p)) return p;
        return null;
    }

    [STAThread]
    static void Main()
    {
        try
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PipelineDesign");
            Directory.CreateDirectory(dir);
            string html = Path.Combine(dir, "pipeline-design.html");
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("app.html"))
            using (FileStream f = File.Create(html))
                s.CopyTo(f);

            string url = new Uri(html).AbsoluteUri;
            string browser = FindBrowser();
            if (browser == null) { Process.Start(html); return; } // varsayılan tarayıcıda aç
            string profile = Path.Combine(dir, "profil");
            string args = "--app=\"" + url + "\" --user-data-dir=\"" + profile + "\" --no-first-run --no-default-browser-check --window-size=1440,900";
            Process.Start(new ProcessStartInfo(browser, args) { UseShellExecute = false });
        }
        catch (Exception e)
        {
            MessageBox.Show("Program açılamadı:\n" + e.Message, "Boru Hattı Tasarımı", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
