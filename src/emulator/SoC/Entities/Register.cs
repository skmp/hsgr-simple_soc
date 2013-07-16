using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SoC.Entities
{
    public class Register :ICloneable
    {
        public string Name { get; private set; }
        public int Number { get; private set; }

        public Register(string name, int value)
        {
            Name = name;
            Number = value;
            if (value < 0 || value > Math.Pow(2, 4))
                throw new Exception("Only 16 registers are permitted");
        }

        public object Clone()
        {
            Register r = new Register(Name, Number);
            return r;
        }

        private ListViewItem _listViewItem;
        public ListViewItem ListViewItem
        {
            get
            {
                if (_listViewItem == null)
                {
                    _listViewItem = new ListViewItem();

                    _listViewItem.SubItems[0].Text = Name;
                    _listViewItem.SubItems.Add("");
                }

                return _listViewItem;
            }
        }
    }
}
