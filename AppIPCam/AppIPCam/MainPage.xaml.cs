using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProtocolType = System.Net.Sockets.ProtocolType;

namespace AppIPCam
{
    public partial class MainPage : ContentPage
    {
        TcpClient client;
        bool Run = true;
        // http://stackoverflow.com/questions/1081860/reading-data-from-an-open-http-stream

        public MainPage()
        {
            InitializeComponent();
            string ip = "192.168.43.96";
            IpEntry.Text = ip;
        }
        private void Disconnect(object sender, EventArgs e)
        {
            client.Close();
            Run = false;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {

            var childSocketThread = new Thread(() =>
            {
                Run = true;
                client = new TcpClient(IpEntry.Text, 8080);

                NetworkStream stream = client.GetStream();

                BinaryReader binaryReader = new BinaryReader(stream);

                while (Run)
                {
                    try
                    {
                        int recv = 0;
                        recv = binaryReader.ReadInt32();
                        Debug.WriteLine("Image size : " + recv);

                        if (recv > 0 && recv < 150000)
                        {
                            byte[] tab = new byte[recv];

                            tab = binaryReader.ReadBytes(recv);
                            //Debug.WriteLine(tab);
                        }
                        else
                            Debug.WriteLine("La taille que t'as envoyé n'est pas bonne");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            });
            childSocketThread.Start();
        }
    }
}
