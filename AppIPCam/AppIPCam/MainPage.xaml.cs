using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;
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

        public MainPage()
        {
            InitializeComponent();
            string ip = "192.168.43.79";
            IpEntry.Text = ip;
        }
        private void Disconnect(object sender, EventArgs e)
        {
            client.Close();
            Run = false;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Run = true;
            var childSocketThread = new Thread(async () =>
            {
                client = new TcpClient(IpEntry.Text, 8080);

                NetworkStream stream = client.GetStream();

                while (Run)
                {
                    BinaryReader bR = new BinaryReader(stream);
                    string lenght = "";
                    try
                    {
                        lenght = bR.ReadString();
                    } catch (Exception ex) {
                        Run = false;
                    }

                    if (lenght != "") {
                        int len = Convert.ToInt32(lenght);
                        Debug.WriteLine(len);
                        try {
                            byte[] tab = new byte[len];
                            tab = bR.ReadBytes(len);

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ImageSource source = ImageSource.FromStream(() => new MemoryStream(tab));
                                StreamImage.Source = source;

                            });
                        } catch (Exception ex) {
                            Debug.WriteLine(ex);
                        }
                    }
                    await Task.Delay(40);
                }
            });
            childSocketThread.Start();
        }
    }
}
