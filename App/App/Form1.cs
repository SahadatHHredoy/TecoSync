using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocket4Net;
using zkemkeeper;

namespace App
{
    public partial class Home : Form
    {
        WebSocket websocket;
       public static CZKEM zkTeco=new CZKEM();
        string serialNo = string.Empty;
  

        public Home()
        {
            InitializeComponent();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
          
            ShowLog("System Start");
            SetUpServer();
            SetupDevice();
        }
        #region Setup Device
        public void SetupDevice()
        {
            string ipAddress = txtIpAddress.Text;
            string port = txtPort.Text;
            if (zkTeco.Connect_Net(ipAddress, Convert.ToInt32(port)))
            {
               // zkTeco.OnDisConnected += ZkTeco_OnDisConnected;
                ShowLog("Device Connected");
            }
            else
            {
                ShowLog("Device Connection Failed");
            }

        }

        private void ZkTeco_OnDisConnected(object value)
        {
          
            //zkTeco.GetSerialNumber(1, ref serialNo);
            serialNo = "ZX0006827500";
            var macInfo = new MachineReg()
            {
                cmd ="reg",
                sn = serialNo,
                devinfo = new DeviceInfo()
                {
                    modelname = "tfs30",
                    usersize =3000,
                    fpsize =3000,
                    cardsize =3000,
                    pwdsize =3000,
                    logsize =100000,
                    useduser=1000,
                    usedfp =1000,
                    usedcard =2000,
                    usedpwd =400,
                    usedlog=10000,
                    usednewlog =5000,
                    fpalgo= "thbio3.0",
                    firmware = "th600w v6.1",
                    time = DateTime.Now.GetTimeFormat()
                }
            };
            websocket.Send(macInfo.GetJsonFormat());
          
        }
        #endregion

        #region Setup Server
        public void SetUpServer()
        {
            string serverAddress = txtServerAddress.Text;
            string serverPort = txtServerPort.Text;
            string wsAddress = "ws://" + serverAddress + ":" + serverPort;
            websocket = new WebSocket(wsAddress);
            ShowLog("Server address:" + wsAddress);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += Websocket_Error;
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += Websocket_MessageReceived;
            websocket.Open();
        }
        private void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            ShowLog("erro");
        }

        private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ShowLog("Message Rcv :"+e.Message);
            string message = e.Message;
            if (message.Contains("getnewlog"))
            {
                var logs = new Logs()
                {
                    ret = "getnewlog",
                    result = true,
                    count = 2,
                    from = 1,
                    to = 3,
                    record = new List<Log>()
                    {
                        new Log()
                        {
                            enrollid =8,
                            time = new DateTime(2020,1,1,10,10,40).GetTimeFormat(),
                            mode =0,
                            inout=0,
                            @event=0
                        },
                        new Log()
                        {
                             enrollid =8,
                            time = new DateTime(2020,1,1,18,10,50).GetTimeFormat(),
                            mode =0,
                            inout=0,
                            @event=0
                        },
                    }
                    
                };
           
                websocket.Send(logs.GetJsonFormat());
            }
            else if (message.Contains("getalllog"))
            {

            }
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            ShowLog("Close");
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            serialNo = "ZX0006827500";
            string regString = "{\"cmd\":\"reg\",\"sn\":\"" + serialNo + "\"," +
                                    "\"devinfo\":{" +
                                    "\"modelname\":\"tfs30\"," +
                                    "\"usersize\":3000," +
                                    "\"fpsize\":3000," +
                                    "\"cardsize\":3000," +
                                    "\"pwdsize\":3000," +
                                    "\"logsize\":100000," +
                                    "\"useduser\":1000," +
                                    "\"usedfp\":1000," +
                                    "\"usedcard\":2000," +
                                    "\"usedpwd\":400," +
                                    "\"usedlog\":100000," +
                                     "\"usednewlog\":5000," +
                                     "\"fpalgo\":\"thbio3.0\"," +
                                     "\"firmware\":\"th600w v6.1\"," +
                                     "\"time\":\"" + DateTime.Now.ToString() + "\"" +
                                     "}" +
                                "}";
            websocket.Send(regString);
        }
        #endregion
        #region Show Log
        public void ShowLog(string log)
        {


            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new MethodInvoker(delegate
                {
                    if (listBox1.Items.Count > 1000)
                    {
                        listBox1.Items.Clear();
                    }
                    listBox1.Items.Add(log);
                    int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                    listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                }));
            }
            else
            {
                if (listBox1.Items.Count > 1000)
                {
                    listBox1.Items.Clear();
                }
                listBox1.Items.Add(log);
                int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);

            }



        }
        #endregion
    }
}
