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

        private void setLabel(string msg) {
            lbConsole.Items.Insert(0, lblStatus.Text);
            lblStatus.Text = msg;
        }

        public frmMain(eventHandler eh) {
            evth = eh;
            InitializeComponent();
            evth.onMessage += evth_onMessage;
            evth.LogMessage(this, new LogEventArgs("Main thread running"));
            evth.onDataReady += evth_onDataReady;
        }

        private void evth_onMessage(object sender, LogEventArgs args) {
            Control ctl = lblStatus;
            if (ctl.InvokeRequired)
                ctl.BeginInvoke(new SetLabelCallback(setLabel), new object[] { args.Message });
            else
                setLabel(args.Message);
        }

        private void evth_onDataReady(object sender, DataReadyEventArgs e) {
            Control ctl;
            uint channel = (uint)(int)e.Data[0];
            bool value = (bool)e.Data[1];
            if (btnStatus[channel] != value) {
                btnStatus[channel] = value;
                if (channel == 0) ctl = btnChan0; else ctl = btnChan1;
                if (value) ctl.BackColor = Color.Black; else ctl.BackColor = Color.Red;
            }

        }

        private void button1_Click(object sender, EventArgs e) {
            evth.LJU3Request(LJU3Commands.PUT_ANALOG_ENABLE_PORT, new object[] { 0, 0, 16 });
        }
    }
}
