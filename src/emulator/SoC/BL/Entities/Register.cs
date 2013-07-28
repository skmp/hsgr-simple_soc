using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SoC.BL.Entities
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Value
    {
        [FieldOffset(0)]
        public UInt16 UValue;

        [FieldOffset(0)]
        public Int16 SValue;
    };

    public class Register : ICloneable
    {
        public int Number;
        public Value Value;
        public string Name 
        {
            get
            {
                return "r" + Number.ToString();
            }
        }

        public Register(int number, UInt16 value)
        {
            Number = number;
            if (number < 0 || number > Math.Pow(2, 4))
                throw new Exception("Only 16 registers are permitted");

            Value = new Value();
            Value.UValue = value;
        }

        public object Clone()
        {
            Register r = new Register(Number, 0);
            return r;
        }



        public string ValueString
        {
            get
            {
                return "0x" + Value.UValue.ToString("X").PadLeft(4,'0') + " (" + Value.SValue.ToString() + ")";
            }
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
                    _listViewItem.SubItems.Add(ValueString);
                }

                return _listViewItem;
            }
        }
    }
}
