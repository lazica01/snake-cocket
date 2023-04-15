using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace snake_socket
{
    public partial class Form1 : Form
    {
        int port;
        string password;

        public static PictureBox[,] pictureBoxMap;
        public int i, j, size = 60;
        public static Dictionary<string, Image> images = new Dictionary<string, Image>();
        RichTextBox richTextBoxLog;
        Thread t;
        Dictionary<string, Player> map = new Dictionary<string, Player>();



        public Form1()
        {
            InitializeComponent();
        }

        List<Player> pList = new List<Player>();
        
        private void LoadImages(string s)
        {
            string[] buffer = Directory.GetFiles(s);
            foreach (string path in buffer)
            {
                string[] buffer2 = path.Split('\\');
                string name = buffer2[buffer2.Length - 1].Split('.')[0];
                Image img = Image.FromFile(path);
                images.Add(name, img);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages(@"..\..\..\Slike");
            //CreateMap(10, 10);
            this.KeyDown += Keydown;

            







            timer1.Start();
        }

        private void Keydown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Up:
                    pList[0].smer = 1;
                    break;
                case Keys.Down:
                    pList[0].smer = -1;
                    break;
                case Keys.Left:
                    pList[0].smer = -2;
                    break;
                case Keys.Right:
                    pList[0].smer = 2;
                    break;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int n = 10, m = 10;
            if (textBoxPassword.Text == "") return;
            password = textBoxPassword.Text;
            port = int.Parse(textBoxPort.Text);
            label1.Dispose();
            label2.Dispose();
            textBoxPassword.Dispose();
            textBoxPort.Dispose();
            buttonStart.Dispose();
            this.Size = new Size(size*n+16+200, size*m+39);

            richTextBoxLog = new RichTextBox();
            richTextBoxLog.Size = new Size(200, size * m);
            richTextBoxLog.Location = new Point(size*n, 0);
            richTextBoxLog.ReadOnly = true;
            richTextBoxLog.BorderStyle = BorderStyle.None;
            Controls.Add(richTextBoxLog);


            t = new Thread(new ThreadStart(MultiPlayer));
            t.Start();

            CreateMap(n, m);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(pList != null)
            foreach (Player p in pList) {
                p.Move();
            }
        }
        void Log(string s)
        {
            if (richTextBoxLog.InvokeRequired)
            {
                richTextBoxLog.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBoxLog.Text += s + "\n";
                });

            }
            else richTextBoxLog.Text += s + "\n";
        }
        void MultiPlayer()
        {
            byte[] data = new byte[1024];
            try
            {

                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                Log("Waiting Connection..");

                soc.Bind(new IPEndPoint(IPAddress.Any, port));


                IPEndPoint sender1 = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)sender1;
                do
                {
                    data = new byte[1024];
                    int recv = soc.ReceiveFrom(data, ref Remote);
                    string text = Encoding.ASCII.GetString(data, 0, recv);
                    ProcessPacket(((IPEndPoint)Remote).Address, text);
                    Log(((IPEndPoint)Remote).Address.ToString() + " : " + text);
                } while (Text != "x");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        void ProcessPacket(IPAddress ipAddress, string data)
        {
            string ip = ipAddress.ToString();
            if (map.ContainsKey(ip))
            {
                if(data == password)
                    Log("Already Loged in");
                else
                {
                    try
                    {
                        map[ip].smer = int.Parse(data);
                    }
                    catch(Exception ex) { }
                }
            }
            else
            {
                if (data == password)
                {
                    Log("Success Login");
                    Player p = new Player(images["snake"]);
                    pList.Add(p);
                    map.Add(ip, p);
                }
                else
                    Log("Fail Login");
            }
        }

        private void CreateMap(int n, int m) {
            pictureBoxMap = new PictureBox[n, m];
            for (i = 0; i < n; i++) {
                for (j = 0; j < m; j++) {
                    pictureBoxMap[i, j] = new PictureBox();
                    pictureBoxMap[i, j].Location = new Point(j * size, i * size);
                    pictureBoxMap[i, j].Size = new Size(size, size);
                    pictureBoxMap[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                    pictureBoxMap[i, j].BackgroundImage = images["default"];
                    Controls.Add(pictureBoxMap[i, j]);
                }
            }
        }
    }
}
