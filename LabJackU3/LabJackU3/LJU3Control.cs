using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using LabJack;
using LabJack.LabJackUD;

namespace LabJackU3 {

    public enum LJU3Commands {
        GET_CONFIG, PIN_CONFIGURATION_RESET, PUT_ANALOG_ENABLE_PORT, GET_DIGITAL_BIT, PUT_DIGITAL_BIT
    }

    public class DataReadyEventArgs : EventArgs {
        public LJU3Commands Command { get; private set; }
        public object[] Data { get; private set; }
        public DataReadyEventArgs(LJU3Commands command, object[] data) {
            Command = command;
            Data = data;
        }
    }
    public delegate void DataReadyCallback(object sender, DataReadyEventArgs e);


    public class LJU3Command {
        public LJU3Commands Command;
        public object[] Args;
        public LJU3Command(LJU3Commands command, object[] args) { Command = command; Args = args; }
        ~LJU3Command() { }
    }

    public class LJU3Control {
        private eventHandler evth;
        private U3 u3;
        private bool isValid = false;
        private Queue<LJU3Command> cmdQueue = new Queue<LJU3Command>();


        public LJU3Control(eventHandler eh) {
            evth = eh;
            evth.onLJU3Request += Evth_onLJU3Request;
        }

        private void Evth_onLJU3Request(object sender, LJU3RequestArgs request) {
            Execute(request.Command, request.Args);
        }

        ~LJU3Control() { evth.LogMessage(this, new LogEventArgs(LogLevel.DEBUG, "Thread aborted.")); }

        public void ShowErrorMessage(LabJackUDException e) {
            //Called when an error occurred. When the error is a group error (communication/fatal), abort.
            evth.LogMessage(this, new LogEventArgs(LogLevel.ERROR, e.ToString()));
            if (e.LJUDError > U3.LJUDERROR.MIN_GROUP_ERROR) {
                Environment.Exit(-1);                                   //TODO: Test this! or just abort the thread.
            }
        }

        private void Initialize() {
            double value=0;
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "Initializing."));
            

            try {  //Open the first found LabJack U3 through USB.
                u3 = new U3(LJUD.CONNECTION.USB, "0", true);
                isValid = true;
            } catch (Exception ex) {
                evth.LogMessage(this, new LogEventArgs(LogLevel.ERROR, "Initialize exception: " + ex.Message));
                return;
            }
            value = LJUD.GetDriverVersion();
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "UD Driver Version: " + value.ToString()));
            LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, LJUD.CHANNEL.HARDWARE_VERSION, ref value, 0);
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "Hardware Version: " + value));
            LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, LJUD.CHANNEL.FIRMWARE_VERSION, ref value, 0);
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "Firmware Version: " + value));
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "Initialized."));

            Execute(LJU3Commands.PIN_CONFIGURATION_RESET, new object[] { });            //pin assignments in factory default condition.
            Execute(LJU3Commands.PUT_ANALOG_ENABLE_PORT, new object[] { 0, 0, 16 });    //all assignments digital input.


        }

        public void Execute(LJU3Commands command, object[] args) {
            if (!isValid) {
                evth.LogMessage(this, new LogEventArgs(LogLevel.ERROR, "Not initialized, aborting."));
                return;
            }
            cmdQueue.Enqueue(new LJU3Command(command, args));
        }

        private void HandleCommand(LJU3Commands command, object[] args) {
            LogLevel loglevel = LogLevel.INFO;
            switch (command) {
                case LJU3Commands.GET_CONFIG:
                    double value = 0;
                    LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, (LJUD.CHANNEL)args[0], ref value, 0);
                    break;
                case LJU3Commands.PIN_CONFIGURATION_RESET:
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PIN_CONFIGURATION_RESET, 0, 0, 0);
                    break;
                case LJU3Commands.PUT_ANALOG_ENABLE_PORT:
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_ANALOG_ENABLE_PORT, (LJUD.CHANNEL)args[0], Convert.ToDouble(args[1]), (int)args[2]);
                    break;
                case LJU3Commands.GET_DIGITAL_BIT:
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.GET_DIGITAL_BIT, (LJUD.CHANNEL)args[0], Convert.ToDouble(args[1]), (int)args[2], Convert.ToDouble(args[3]));
                    loglevel = LogLevel.RAW;
                    break;
                case LJU3Commands.PUT_DIGITAL_BIT:
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_DIGITAL_BIT, (LJUD.CHANNEL)args[0], Convert.ToDouble(args[1]), 0, 0);
                    break;
                default:
                    throw (new Exception("Unhandled command."));
                    //break;
            }
            evth.LogMessage(this, new LogEventArgs(loglevel, "Command executed: " + command.ToString()));
        }

        private void DeQueue() {
            while (cmdQueue.Count > 0) {
                LJU3Command command = cmdQueue.Dequeue();
                HandleCommand(command.Command, command.Args);
            }
        }

        private void HandleResult(LJUD.IO ioType, LJUD.CHANNEL channel, double value) {


            switch (ioType) {
                case LJUD.IO.GET_DIGITAL_BIT:
                    object[] data = new object[] { (int)channel, Convert.ToBoolean(value) };
                    evth.LogMessage(this, new LogEventArgs(LogLevel.RAW, "Result handled: " + ioType.ToString()));
                    evth.DataReady(this, new DataReadyEventArgs(LJU3Commands.GET_DIGITAL_BIT, data));
                    break;
                case LJUD.IO.GET_CONFIG:
                    break;
                default:
                    evth.LogMessage(this, new LogEventArgs(LogLevel.ERROR, channel.ToString() + " Unhandled Result"));
                    break;
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]        //This is used to supress Exception messages from the LabJack library.
        private void GetResults() {
            LJUD.IO ioType = 0;
            double dblValue = 0;
            LJUD.CHANNEL channel = 0;
            int dummyInt = 0;
            double dummyDouble = 0;
            bool finished = false;
            try {
                LJUD.GetFirstResult(u3.ljhandle, ref ioType, ref channel, ref dblValue, ref dummyInt, ref dummyDouble);
            } catch (LabJackUDException e) {
                ShowErrorMessage(e);
            }
            while (!finished) {
                HandleResult(ioType, channel, dblValue);
                try {
                    LJUD.GetNextResult(u3.ljhandle, ref ioType, ref channel, ref dblValue, ref dummyInt, ref dummyDouble);
                } catch (LabJackUDException e) {
                    // If we get an error, report it.  If the error is NO_MORE_DATA_AVAILABLE we are done
                    if (e.LJUDError == U3.LJUDERROR.NO_MORE_DATA_AVAILABLE) {
                        finished = true;
                    } else ShowErrorMessage(e);
                }
            }
        }


        public void Main() {
            Initialize();


            while (isValid) {
                Execute(LJU3Commands.GET_DIGITAL_BIT, new object[] { 0, 0, 0, 0 });
                Execute(LJU3Commands.GET_DIGITAL_BIT, new object[] { 1, 0, 0, 0 });
                DeQueue();
                LJUD.GoOne(u3.ljhandle);
                Thread.Sleep(10);
                GetResults();

            }

        }
    }
}
