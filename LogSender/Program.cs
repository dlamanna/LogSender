using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogSender
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            ArrayList logPaths = new ArrayList();
            ArrayList fileStreamLists = new ArrayList();
            String IPAdress = "192.168.0.15";
            int sleepTimer = 1000;

            logPaths.Add(@"D:\Program Files (x86)\DesktopShell\DesktopShell.log");
            logPaths.Add(@"D:\Program Files (x86)\IStation\ISDemo\IStation.log");
        
            foreach (String s in logPaths)
            {
                var fs = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                fileStreamLists.Add(fs);
            }

            while (true)
            {
                using (var reader = new StreamReader((FileStream)fileStreamLists[0]))
                {
                    using (var reader2 = new StreamReader((FileStream)fileStreamLists[1]))
                    {
                        var line1 = reader.ReadLine();
                        var line2 = reader2.ReadLine();

                        if (!String.IsNullOrWhiteSpace(line1))
                            Connect(IPAdress, "[DesktopShell] " + line1);

                        if (!String.IsNullOrWhiteSpace(line2))
                            Connect(IPAdress, "[IStation] " + line2);
                    }
                }

                System.Threading.Thread.Sleep(sleepTimer);
            }
        }

        static int getNumLines(String path)
        {
            return File.ReadAllLines(path).Length;
        }

        static void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.Read();
        }
    }
}
