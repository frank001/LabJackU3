using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LabJackU3 {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static eventHandler evtHandler;
        [STAThread]
        static void Main() {
            evtHandler = new eventHandler();
            MainForm mainForm = new MainForm();
            Thread mainThread = new Thread(new ThreadStart(mainForm.Main));
            mainThread.Start();
            while (!mainThread.IsAlive) ;
            Thread.Sleep(100);

            LJU3Control lju3Control = new LJU3Control(evtHandler);
            Thread lju3Thread = new Thread(new ThreadStart(lju3Control.Main));
            lju3Thread.Start();


            while (mainThread.IsAlive) {
                Thread.Sleep(10);
            }
            mainThread.Join();
            lju3Thread.Abort();
            while (lju3Thread.IsAlive) ;
            lju3Thread.Join();

        }

        public class MainForm {
            public void Main() {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(evtHandler));
            }
        }

    }
}
