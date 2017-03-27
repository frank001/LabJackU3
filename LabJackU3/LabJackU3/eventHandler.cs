using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LabJack.LabJackUD;

namespace LabJackU3 {

    public class LogEventArgs : EventArgs {
        public string Message { get; private set; }
        public LogEventArgs(string message) {
            Message = message;
        }
    }
    public delegate void LogEventCallback(object sender, LogEventArgs args);

    public class LJU3RequestArgs : EventArgs {
        public LJU3Commands Command;
        public object[] Args;
        public LJU3RequestArgs(LJU3Commands command, object[] args) {
            Command = command;
            Args = args;
        }
    }
    public delegate void LJU3RequestCallback(object sender, LJU3RequestArgs args);


    public class eventHandler {
        public event LogEventCallback onMessage;
        public event DataReadyCallback onDataReady;
        public event LJU3RequestCallback onLJU3Request;

        public eventHandler() { }

        public void LogMessage(object sender, LogEventArgs args) {
            Console.WriteLine(sender.ToString() + ": " + args.Message);
            if (onMessage == null) return;
            onMessage(sender, args);
        }

        public void DataReady(object sender, DataReadyEventArgs args) {
            if (onDataReady == null) return;
            onDataReady(sender, args);
        }

        public void LJU3Request(LJU3Commands lju3Command, object[] param) {
            if (onLJU3Request == null) return;
            onLJU3Request(this, new LJU3RequestArgs(lju3Command, param));
        }

    }
}
