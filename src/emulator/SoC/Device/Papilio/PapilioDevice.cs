using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SoC.Properties;

namespace SoC.Device.Papilio
{
    class PapilioDevice : IDevice
    {
        private SerialPort comPort = new SerialPort();

        #region public string DeviceName
        public string DeviceName
        {
            get { return "Papilio"; }
        }
        #endregion
        #region public bool CanReset
        public bool CanReset
        {
            get { return true; }
        }
        #endregion
        #region public bool CanHalt
        public bool CanHalt
        {
            get { return true; }
        }
        #endregion
        #region public bool CanResume
        public bool CanResume
        {
            get { return true; }
        }
        #endregion
        #region public bool CanStep
        public bool CanStep
        {
            get { return true; }
        }
        #endregion
        #region public bool CanConfigure
        public bool CanConfigure
        {
            get { return true; }
        }
        #endregion
        #region public bool CanDisplay
        public bool CanDisplay
        {
            get { return false; }
        }
        #endregion
        #region public void Configure()
        public void Configure()
        {
            PapilioConfiguration w = new PapilioConfiguration();
            w.Show();
        }
        #endregion
        #region public void Instantiate()
        public void Instantiate()
        {
            //first check if the port is already open
            //if its open then close it
            if (comPort.IsOpen == true) 
                comPort.Close();

            //set the properties of our SerialPort Object
            comPort.PortName = PapilioSettings.Default.PapilioPort;   //PortName
            comPort.BaudRate = int.Parse(PapilioSettings.Default.PapilioBaudRate);    //BaudRate
            comPort.DataBits = int.Parse(PapilioSettings.Default.PapilioDataBits);    //DataBits
            comPort.Parity = (Parity)Enum.Parse(typeof(Parity), PapilioSettings.Default.PapilioParity);    //Parity
            comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), PapilioSettings.Default.PapilioStopBits);    //StopBits
            //now open the port
            comPort.Open();
        }
        #endregion
        public byte WriteCommand(byte command)
        {
            byte[] sendBuffer = new byte[1];
            byte[] recvBuffer = new byte[1];

            // send command byte
            sendBuffer[0] = command;
            comPort.Write(sendBuffer, 0, 1);

            // read response byte
            comPort.Read(recvBuffer, 0, 1);

            // return response byte
            return recvBuffer[0];
        }
        #region public void Destroy()
        public void Destroy()
        {
            if (comPort.IsOpen == true)
                comPort.Close();
        }
        #endregion

        #region public void ShowDisplay()
        public void ShowDisplay()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region public override string ToString()
        public override string ToString()
        {
            return DeviceName;
        }
        #endregion
    }
}
