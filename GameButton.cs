using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X0
{
    class GameButton : Button
    {
        public bool IsClear { get; set; }
        public bool IsCross { get; set; }
        public bool IsZero { get; set; }

        public GameButton ()
        {
            Text = "";
            Font = new Font(Font.Name, 30, Font.Style);
            IsClear = true;
            IsCross = IsZero = false;
        }
        public void SetCross()
        {
            Text = "X";
            IsCross = true;
            IsClear = IsZero = false;
        }

        public void SetNull()
        {
            Text = "O";
            IsClear = IsCross = false;
            IsZero = true;
        }

        public void SetClear()
        {
            Text = "";
            IsClear = true;
            IsCross = IsZero = false;
        }

    }
}
