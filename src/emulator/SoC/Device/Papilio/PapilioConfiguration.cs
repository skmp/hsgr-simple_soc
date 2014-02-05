using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using SoC.Properties;

namespace SoC.Device.Papilio
{
    public partial class PapilioConfiguration : Form
    {
        public PapilioConfiguration()
        {
            InitializeComponent();
        }

        // Initialization
        #region private void PapilioConfiguration_Load(object sender, EventArgs e)
        private void PapilioConfiguration_Load(object sender, EventArgs e)
        {
            cboPort.Items.AddRange(SerialPort.GetPortNames());
            cboBaudRate.Items.AddRange(new string[] { "1000000", "3000000" });
            cboDataBits.Items.AddRange(new string[] { "8" });
            cboParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cboStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            LoadSettings();

            if (cboBaudRate.Text == "")
            {
                if (cboPort.Items.Count > 0)
                    cboPort.SelectedIndex = 0;
                if (cboBaudRate.Items.Count > 0)
                    cboBaudRate.SelectedIndex = 0;
                if (cboDataBits.Items.Count > 0)
                    cboDataBits.SelectedIndex = 0;
                if (cboParity.Items.Count > 0)
                    cboParity.SelectedIndex = 0;
                if (cboStopBits.Items.Count > 0)
                    cboStopBits.SelectedIndex = 0;
            }
        }
        #endregion

        // Event handlers
        #region private void btnOK_Click(object sender, EventArgs e)
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }
        #endregion
        #region private void btnCancel_Click(object sender, EventArgs e)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        // Private Helpers
        #region private void SaveSettings()
        private void SaveSettings()
        {
            if (cboPort.SelectedItem != null)
                PapilioSettings.Default.PapilioPort = cboPort.SelectedItem.ToString();

            if (cboBaudRate.SelectedItem != null)
                PapilioSettings.Default.PapilioBaudRate = cboBaudRate.SelectedItem.ToString();

            if (cboDataBits.SelectedItem != null)
                PapilioSettings.Default.PapilioDataBits = cboDataBits.SelectedItem.ToString();

            if (cboParity.SelectedItem != null)
                PapilioSettings.Default.PapilioParity = cboParity.SelectedItem.ToString();

            if (cboStopBits.SelectedItem != null)
                PapilioSettings.Default.PapilioStopBits = cboStopBits.SelectedItem.ToString();

            PapilioSettings.Default.Save();
        }
        #endregion
        #region private void LoadSettings()
        private void LoadSettings()
        {
            try
            {
                cboPort.SelectedItem = PapilioSettings.Default.PapilioPort;
                cboBaudRate.SelectedItem = PapilioSettings.Default.PapilioBaudRate;
                cboDataBits.SelectedItem = PapilioSettings.Default.PapilioDataBits;
                cboParity.SelectedItem = PapilioSettings.Default.PapilioParity;
                cboStopBits.SelectedItem = PapilioSettings.Default.PapilioStopBits;
            }
            catch
            {
            }
        }
        #endregion
    }
}
