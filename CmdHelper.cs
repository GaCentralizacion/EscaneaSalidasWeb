using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;

namespace PortalClientes.Old_App_Code
{
    public class CmdHelper
    {
        //This will protect the server from runaway processes
        //If a pdf page cannot render by this time, then abort the rendering
        static int CmdTimeout = 300; //30; //segundos

    
    public static string ExecuteCMD(string cmd, string args) 
    {
        string res="";
        try
        {
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.FileName = cmd;
                myProcess.StartInfo.Arguments = args;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();

                ThreadedKill(myProcess.Id);
                myProcess.PriorityClass = ProcessPriorityClass.Normal;
                res = myProcess.StandardOutput.ReadToEnd();
                myProcess.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return res;
    }

    public static void KillProcessAfterTimeout(int pid)
    {
        try
        {
            Process pProcess = System.Diagnostics.Process.GetProcessById(pid);
            DateTime expiration = DateTime.Now.AddSeconds(CmdTimeout);
            while ((DateTime.Now < expiration))
            {
                Thread.Sleep(100);
                if (pProcess == null)
                {
                    return;
                }
            }
            if (pProcess != null)
            {
                pProcess.Kill();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public static void ThreadedKill(int pid)
    {
        //Thread thread = new Thread(() => download(filename)); 
        Thread myThread = new Thread(() => KillProcessAfterTimeout(pid)); 
        myThread.Start();

        //Thread myThread = new Thread(new ParameterizedThreadStart(KillProcessAfterTimeout));
        //var myThread = new Thread(KillProcessAfterTimeout); // equivalent  
        //myThread.Start(pid);
    }

    }
}
