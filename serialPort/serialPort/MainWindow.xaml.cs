using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;

namespace serialPort
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sp = new SerialPort();
        DispatcherTimer dt;
        int open = 0;
        int connect = 0;
        SerialPort connectPort;
        string CheckPortName=null;
        string[] ports;
        public MainWindow()
        {
            InitializeComponent();
            dt = new DispatcherTimer();
            dt.Tick += runTimer;
            dt.Interval = new TimeSpan(1000 * 8000);
            dt.Start();
        }

        public void checkCOM()
        {
            // Get a list of serial port names.
            ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");
            Console.WriteLine(SerialPort.GetPortNames().Count());
            
            // Display each port name to the console.
            foreach (string port in ports)
            {
                Console.WriteLine(port);
                
                SerialPort mySerialPort = new SerialPort(port);

                    mySerialPort.BaudRate = 38400;
                    mySerialPort.Parity = Parity.None;
                    mySerialPort.StopBits = StopBits.One;
                    mySerialPort.DataBits = 8;
                    mySerialPort.Handshake = Handshake.None;
                    mySerialPort.RtsEnable = true;
                   




                    if (CheckPortName == null)
                    {
                    try
                    {
                     
                        mySerialPort.Open();
                        //mySerialPort.Write("startconnect");
                        mySerialPort.Write("Send");
                        Console.WriteLine(mySerialPort.BytesToRead);
                        if (mySerialPort.BytesToRead != 0)
                        {
                            CheckPortName = port;
                            Console.WriteLine("已確認COM");

                        }
                        mySerialPort.Close();
                        System.Threading.Thread.Sleep(300);
                    }
                    catch(IOException)
                    {
                        mySerialPort.Close();
                        System.Threading.Thread.Sleep(300);
                    }

                        

                }


            }

    

            //Console.ReadLine();
            //for (int i = 1; i < 16; i++)
            //{
            //    sp.BaudRate = 9600; 
            //    sp.PortName = "COM" + i.ToString();
            //    if (sp.IsOpen==true)
            //    {
            //        Console.WriteLine("COM" + i + " 已連結");
            //        break;
            //    }
            //    else
            //    {
            //        Console.WriteLine("COM"+i+"並無連結");
            //    }
            //}
        }
        public void Connect()
        {
            if (CheckPortName != null&&connect==0)
            {
                Console.WriteLine("開始連接"+CheckPortName);
                connectPort = new SerialPort(CheckPortName);

                connectPort.BaudRate = 38400;
                connectPort.Parity = Parity.None;
                connectPort.StopBits = StopBits.One;
                connectPort.DataBits = 8;
                connectPort.Handshake = Handshake.None;
                connectPort.RtsEnable = true;
                connectPort.Open();
                connect = 1;
                Console.WriteLine("連接完成");
            }
        }
        public void checkClick()
        {
            if (connect == 1)
            {
                if (connectPort.ReadExisting().Contains("isClick"))
                { 
                    Console.WriteLine(connectPort.BytesToRead);
                    Console.WriteLine("偵測到點擊");
                    //放入左鍵點擊
                }

            }
        }
        public void checkDisconnect()
        {
            if (connect == 1)
            {
                System.Threading.Thread.Sleep(200);
                Console.WriteLine (connectPort.BytesToRead);
                string[] DisconnectPort = SerialPort.GetPortNames();
                if (DisconnectPort.Count() != 0&&connectPort.IsOpen)
                {
                    if (connectPort.BytesToRead != 0)
                    {
                        Console.WriteLine("接收到Arduino持續訊號");
                        try { 
                            Console.WriteLine("回傳到Arduino");
                            connectPort.Write("skdol");
                            System.Threading.Thread.Sleep(100);
                            checkClick();
                        }
                        catch (IOException)
                        {
                            connectPort.Close();
                            System.Threading.Thread.Sleep(500);
                            connectPort.Dispose();
                            connect = 0;
                            CheckPortName = null;
                            Console.WriteLine("PORT已移除");

                        }
                    }
                    else
                    {
                        connectPort.Write("unplug");
                        Console.WriteLine("PORT已移除");
                        connectPort.Close();
                        connectPort.Dispose();
                        System.Threading.Thread.Sleep(300);
                        connect = 0;
                        CheckPortName = null;
                        ports = null;
                    
                    }


                }
                else 
                {
                    Console.WriteLine("PORT已移除");
                    connectPort.Close();
                    connectPort.Dispose();
                    System.Threading.Thread.Sleep(500);
                    connect = 0;
                    CheckPortName = null;
                    
                }
            }

        }
        public void runTimer(object sender, EventArgs e)
        {
            checkCOM();
            Connect();
            checkDisconnect();


           
        }
    }
}
