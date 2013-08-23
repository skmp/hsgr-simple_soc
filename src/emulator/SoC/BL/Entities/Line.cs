using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections;

namespace SoC.BL.Entities
{
    public class Line
    {
        // Line info
        public string SourceLineOrig { get; private set; }
        public int SourceLineNumber { get; private set; }

        // aggregated line info
        public string ErrorMessage { get; private set; }

        // aseembled line information
        public RomItem[] Opcodes { get; private set; }

        public Line(string line, int line_no)
        {
            this.SourceLineOrig = line;
            this.SourceLineNumber = line_no;
            this.ErrorMessage = null;
            this.Opcodes = new RomItem[0];
        }

        // Normalizes the input line e.g. trimming white space, removing comments, etc
        #region public string[] Parse()
        public string[] Parse()
        {
            string sourceLine = SourceLineOrig;

            // Trim the comments
            int p = sourceLine.IndexOf(';');
            if (p >= 0)
            {
                sourceLine = sourceLine.Substring(0, p);
            }

            // Trim the white space from start and end
            sourceLine = sourceLine.Trim();

            // Convert all comma to space
            sourceLine = Regex.Replace(sourceLine, @"\,", " ");

            // Convert all white space to single space
            sourceLine = Regex.Replace(sourceLine, @"\s+", " ");

            // Split and return the line tokens
            if (!string.IsNullOrEmpty(sourceLine))
                return sourceLine.Split(' ');

            return null;
        }
        #endregion

        public void SetError(string message)
        {
            ErrorMessage = message;
        }

        public void AddRomItem(RomItem ri)
        {
            ArrayList arl = new ArrayList(Opcodes);
            arl.Add(ri);
            Opcodes = (RomItem[])arl.ToArray(typeof(RomItem));
        }

        private ListViewItem[] _listViewItem;
        public ListViewItem[] ListViewItem
        {
            get
            {
                if (_listViewItem == null)
                {
                    ArrayList arl = new ArrayList();
                    ListViewItem lvi = null;

                    if (Opcodes.Length == 0)
                    {
                        lvi = new ListViewItem();
                        lvi.SubItems[0].Text = "";
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add(SourceLineOrig);
                        lvi.SubItems.Add(ErrorMessage);
                        if (!String.IsNullOrEmpty(ErrorMessage))
                        {
                            lvi.BackColor = Color.Red;
                            lvi.ForeColor = Color.White;
                        }
                        arl.Add(lvi);
                    }
                    else
                    {
                        foreach (RomItem ri in Opcodes)
                        {
                            lvi = new ListViewItem();
                            lvi.SubItems[0].Text = (ri.Opcode != null && ri.Opcode.Pseudo == false) ? "0x" + ri.Address.ToString("X").PadLeft(4, '0') : "";
                            lvi.SubItems.Add((ri.Opcode != null && ri.Opcode.Pseudo == false) ? "0x" + ri.Opcode.GetOpcodeValue().ToString("X").PadLeft(4, '0') : "");
                            lvi.SubItems.Add((ri.Opcode != null) ? ri.Opcode.GetNormalizedSourceLine(10) : SourceLineOrig);
                            if (!String.IsNullOrEmpty(ErrorMessage))
                            {
                                lvi.SubItems.Add(ErrorMessage);
                                lvi.BackColor = Color.Red;
                                lvi.ForeColor = Color.White;
                            }
                            else
                            {
                                lvi.SubItems.Add(ri.Opcode.SourceLine.SourceLineOrig);
                            }

                            arl.Add(lvi);
                        }
                    }

                    _listViewItem = (ListViewItem[])arl.ToArray(typeof(ListViewItem));
                }

                return _listViewItem;
            }
        }

    }
}
