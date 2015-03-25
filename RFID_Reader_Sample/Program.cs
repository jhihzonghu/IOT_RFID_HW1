using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace RFID_Reader_Sample
{
    class Program
    {
        public static String MySQLOutput, SerialID,RFID, InsertNewStudentName,CheckInsert;
        public static SerialPort RFID_Reader = new SerialPort("COM11");
        public static Boolean CheckIfAddtoSQL = false;
        public static MySqlConnection Con;
        public static MySqlCommand cmd;

        static void Main(string[] args)
        {
            InitDB();
            ReadDB();
            InitRFID();

            Console.WriteLine("Press any key to continue...");
            RFID_Reader.DataReceived += RFID_Reader_DataReceived;
            RFID_Reader.Open();

            Console.WriteLine();
            CheckInsert = Console.ReadLine(); 
            if (CheckInsert.Equals("Y") || CheckInsert.Equals("y"))
            {
                Console.WriteLine("--------------INSERT------------");
                InsertNewDataToDB();

            }
            Console.ReadLine();
            RFID_Reader.Close();
            
        }

        private static void ReadDB()
        {
            MySqlDataReader Reader;
            cmd = Con.CreateCommand();
            cmd.CommandText = "SELECT * FROM iot2015 ";
            Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                MySQLOutput = Reader.GetString(0);
                Console.Write(MySQLOutput + "\t");
            }
            Console.Write("\n");
            Con.Close();
        }

        private static void InitRFID()
        {
            RFID_Reader.BaudRate = 9600;
            RFID_Reader.Parity = Parity.None;
            RFID_Reader.StopBits = StopBits.One;
            RFID_Reader.DataBits = 8;
            RFID_Reader.Handshake = Handshake.None;
        }

         static void InitDB()
        {
            string myConnectionString;
            myConnectionString = "server=;uid=;" + "pwd=;database=;";
            Con = new MySqlConnection(myConnectionString);
            Con.Open();
   
        }

        static void RFID_Reader_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            SerialID += sp.ReadExisting().Trim();
            if (SerialID.Length == 8) 
            {
              RFID = SerialID;
              if (CompareWithDB())
              {
                  Console.WriteLine("Do You Want To Add New Student To The List? Y(y)/N(n)");
                  CheckIfAddtoSQL = true;

              }
              else 
              {
                  CheckIfAddtoSQL = false; 
              };
              SerialID="";
            }
            
           
        }

        private static void InsertNewDataToDB()
        {
            string myConnectionString;
            myConnectionString = "server=;uid=;" + "pwd=;database=;";
            MySqlConnection Con = new MySqlConnection(myConnectionString);
            cmd = Con.CreateCommand();
            Con.Open();

            Console.Write("Student Number:");
            String StudentNo = Console.ReadLine();
            Console.Write("Student Name:");
            String StudentName = Console.ReadLine();
            int StartTime = 3250616;
            int StopTime = 3250616;

            cmd.CommandText = "Insert into iot2015(RFID_no,Student_no,Student_Name) values('"+RFID+"','"+StudentNo+"','"+StudentName+"')";
            cmd.ExecuteNonQuery();
            Con.Close();
        }

         static Boolean CompareWithDB()
        {
            if (RFID.Equals(MySQLOutput))
            {
                Console.WriteLine("Login Sucess !");
                return false;  
            }
            else 
            {
                Console.WriteLine("Login Fail !");
               
                return true;  
            }
        }
    }
}
