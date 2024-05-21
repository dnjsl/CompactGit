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

        public static string CallCmdAndOutput(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command + " >> tmp.txt";

            process.StartInfo = startInfo;
            process.Start();

            string result;

            using (StreamReader reader = new StreamReader("tmp.txt"))
            {
                result = reader.ReadToEnd();
            }
            File.Delete("tmp.txt");

            return result;
        }
    }
}
