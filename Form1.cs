using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;// socket.io for .NET (Client)

namespace SocketIoClient
{
    public delegate void UpdateTextBoxMethod(string text);
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Start our socket.io connection and handle incoming data
            socketIoManager();
        }

        private void socketIoManager()
        {
            // Instantiate the socket.io connection
            var socket = IO.Socket("http://192.168.7.2:8888");
            // Upon a connection event, update our status
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                UpdateStatus("Connected");
            });
            // Upon temperature data, update our temperature status
            socket.On("temperature", (data) =>
            {
                var temperature = new { temperature = "" };
                var tempValue = JsonConvert.DeserializeAnonymousType((string)data, temperature);
                UpdateTemp((string)tempValue.temperature);
            });
        }

        private void UpdateStatus(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                UpdateTextBoxMethod del = new UpdateTextBoxMethod(UpdateStatus);
                this.Invoke(del, new object[] { "Connected" });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void UpdateTemp(string text)
        {
            if (this.textBox2.InvokeRequired)
            {
                UpdateTextBoxMethod del = new UpdateTextBoxMethod(UpdateTemp);
                this.Invoke(del, new object[] { text });
            }
            else
            {
                this.textBox2.Text = text;
            }
        }
    }
}
