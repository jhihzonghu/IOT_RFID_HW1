using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RFID_Reader_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort RFID_Reader = new SerialPort("COM7");

            RFID_Reader.BaudRate = 9600;
            RFID_Reader.Parity = Parity.None;
            RFID_Reader.StopBits = StopBits.One;
            RFID_Reader.DataBits = 8;
            RFID_Reader.Handshake = Handshake.None;

            RFID_Reader.DataReceived += RFID_Reader_DataReceived;

            RFID_Reader.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();

            RFID_Reader.Close();
        }

        static void RFID_Reader_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            Console.Write(data);
        }
    }
}
