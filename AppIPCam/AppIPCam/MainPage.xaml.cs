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
using Android.App;
using Android.Net.Wifi;
using Java.Net;
using Xamarin.Forms;
using ProtocolType = System.Net.Sockets.ProtocolType;

namespace AppIPCam
{
	public partial class MainPage : ContentPage
    {
        const int PORT_NO = 8080;
        const string SERVER_IP = "127.0.0.1";

        private string ip;
        // http://stackoverflow.com/questions/1081860/reading-data-from-an-open-http-stream
        public MainPage()
		{
			InitializeComponent();
		    IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

		    if (adresses != null && adresses[0] != null)
		    {
		        ip = adresses[0].ToString();
		    }
		    else
		    {
		    }
		    
		    var childSocketThread = new Thread(() =>
		    {
		        var t = Inet6Address.LocalHost;
                
            });
		    childSocketThread.Start();


            WifiManager wifiManager = (WifiManager)Forms.Context.GetSystemService(Service.WifiService);
		    var iip = wifiManager.ConnectionInfo.IpAddress.ToString();
            Debug.WriteLine("IP : " + iip);
		    Debug.WriteLine("IP : " + iip);
		    Debug.WriteLine("IP : " + iip);
            IpAddr.Text = ip;
		}

	    private void GetStream(object sender, EventArgs e)
	    {

	        //---listen at the specified IP and port no.---
	        IPAddress localAdd = IPAddress.Parse(ip);
	        TcpListener listener = new TcpListener(localAdd, PORT_NO);
	        Console.WriteLine("Listening...");
	        listener.Start();

            //---incoming client connected---
	        var tcpClient = listener.AcceptTcpClient();


            //Socket client = listener.AcceptSocket();
	        Console.WriteLine("Connection accepted.");

	        var childSocketThread = new Thread(() =>
	        {
	            NetworkStream clientStream = tcpClient.GetStream();
                BinaryReader reader = new BinaryReader(clientStream);

	            int size = reader.ReadInt32();
                
                Debug.WriteLine(size);







                /*byte[] data = new byte[100];
	            size = client.Receive(data);
	            Console.WriteLine("Recieved data: ");
	            for (int i = 0; i < size; i++)
	                Console.Write(Convert.ToChar(data[i]));

	            Console.WriteLine();

	            client.Close();*/
	        });
	        childSocketThread.Start();






            //var web = new HttpListener();







            /*web.Prefixes.Add("http://localhost:8080/");

	        Console.WriteLine("Listening..");

	        web.Start();

	        Console.WriteLine(web.GetContext());

	        var context = web.GetContext();

	        var response = context.Response;

	        const string responseString = "<html><body>Hello world</body></html>";

	        var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

	        response.ContentLength64 = buffer.Length;

	        var output = response.OutputStream;

	        output.Write(buffer, 0, buffer.Length);

	        Console.WriteLine(output);

	        output.Close();

	        web.Stop();

	        Console.ReadKey();*/
        }
	}
}
