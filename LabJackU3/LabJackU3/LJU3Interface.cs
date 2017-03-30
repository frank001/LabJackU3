using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabJackU3 {

    public class DigitalIOChangedArgs : EventArgs {
        public Control Ctl;
        public int Channel;
        public bool Value;
        public DigitalIOChangedArgs(int channel, bool value) {
            Channel = channel;
            Value = value;
        }
    }
    public delegate void DigitalIOChangedCallback(object sender, DigitalIOChangedArgs args);

    public class DigitalControl : Control {
        private eventHandler evth;
        public Control Ctl;
        public byte Channel;
        public bool Value;
        public enum eMode { Off, Input, Output };
        public delegate void DigitalIOChangedCallback(object sender, DigitalIOChangedArgs args);
        public event DigitalIOChangedCallback onChange;

        public DigitalControl(eventHandler eh, Control ctl, byte channel, bool value) {
            evth = eh; Ctl = ctl; Channel = channel; Value = value;
            evth.onDigitalIOChange += Evth_onDigitalIOChange;
        }

        private void Evth_onDigitalIOChange(object sender, DigitalIOChangedArgs args) {
            if (onChange == null || args.Channel != Channel) return;
            args.Ctl = Ctl;
            onChange(this, args);
            evth.LogMessage(this, new LogEventArgs(LogLevel.INFO, "Digital IO channel "+ args.Channel.ToString()+" ("+ args.Ctl.Name.ToString() + ") value: " + args.Value.ToString()));
            
        }
    }
    

    class LJU3Interface {


    }
}
