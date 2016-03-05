using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSCSimulator
{
    public class BetterTimer : System.Windows.Forms.Timer
    {
        public BetterTimer() : base()
        { base.Enabled = true; }

        public BetterTimer(System.ComponentModel.IContainer container) : base(container)
        { base.Enabled = true; }

        private bool _Enabled;
        public override bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        protected override void OnTick(System.EventArgs e)
        { if (this.Enabled) base.OnTick(e); }
    }
}
