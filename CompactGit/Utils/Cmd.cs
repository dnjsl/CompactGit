using System.Diagnostics;

namespace CompactGit.Utils
{
    public class Cmd
    {
        public static void CallCmd(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
