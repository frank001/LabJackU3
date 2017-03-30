using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabJack.LabJackUD;

namespace LabJackU3 {
    public partial class frmMain : Form {
        private eventHandler evth;
        public delegate void SetLabelCallback(string msg);
        private bool[] btnStatus = new bool[] { true, true };

        private class ListBoxItem : object {
            public uint Value = 0;
            public string Name = "";
            public ListBoxItem(uint value, string name) {
                Value = value;
                Name = name;
            }
            public override string ToString() {
                return Name;
            }
        }

        public frmMain(eventHandler eh) {
            evth = eh;
            InitializeComponent();

            evth.onMessage += evth_onMessage;

            //Populate combobox DebugLevel with LogLevel enumeration.
            uint[] logLevelValues = (uint[])Enum.GetValues(typeof(LogLevel));
            string[] logLevelNames = Enum.GetNames(typeof(LogLevel));
            for (uint i=0;i< logLevelValues.Length;i++) {
                ListBoxItem lbi = new ListBoxItem(logLevelValues[i], logLevelNames[i]);
                cbDebugLevel.Items.Insert(0, lbi);
            }
            //set combobox DebugLevel to initial value (set in code).
            for (int i=0;i<cbDebugLevel.Items.Count;i++) {
                if (((ListBoxItem)cbDebugLevel.Items[i]).Value==(uint)evth.loglevel) {
                    cbDebugLevel.SelectedIndex = i;
                    break;
                }
            }
            //add eventhandler for when the user changes the log level.
            cbDebugLevel.SelectedIndexChanged += CbDebugLevel_SelectedIndexChanged;

            //eventhandler for digital io
            for (byte i=0;i<gbDigitalIO.Controls.Count;i++) {
                gbDigitalIO.Controls[i].Text = i.ToString();
                DigitalControl digitalControl = new DigitalControl(evth, gbDigitalIO.Controls[i], i, false);
                digitalControl.onChange += DigitalControl_onChange;    
            }
        }

        private void DigitalControl_onChange(object sender, DigitalIOChangedArgs args) {
            if (args.Value) args.Ctl.BackColor = Color.Red; else args.Ctl.BackColor = Color.Black;
        }

        private void CbDebugLevel_SelectedIndexChanged(object sender, EventArgs e) {
            evth.loglevel = (LogLevel)((ListBoxItem)cbDebugLevel.Items[cbDebugLevel.SelectedIndex]).Value;
        }


        ~frmMain() { }

        private void setLabel(string msg) {
            lbConsole.Items.Insert(lbConsole.Items.Count, msg);
            lbConsole.SelectedIndex = lbConsole.Items.Count - 1;
            lblStatus.Text = msg;
        }

        private void evth_onMessage(object sender, LogEventArgs args) {
            Control ctl = lblStatus;
            if (ctl.InvokeRequired)
                ctl.BeginInvoke(new SetLabelCallback(setLabel), new object[] { args.Message });
            else
                setLabel(args.Message);
        }

        private void button1_Click(object sender, EventArgs e) {
            evth.LJU3Request(LJU3Commands.PUT_ANALOG_ENABLE_PORT, new object[] { 0, 1, 16 });
        }

        private void button2_Click(object sender, EventArgs e) {
            evth.LJU3Request(LJU3Commands.PUT_DIGITAL_BIT, new object[] { 2, 1 });
        }
    }
}
