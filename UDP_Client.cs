using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Simple_App
{
    public class UDP_Client: IDisposable
    {
        private IPAddress Address = null!;
        private Socket? listenSocket = null;
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }

        public UDP_Client(int localPort, int remotePort, string ipAddress)
        {
            LocalPort = localPort;
            RemotePort = remotePort;

            if (!IPAddress.TryParse(ipAddress, out Address))
            {
                throw new ArgumentException("Неверный формат IP адреса");
            }

            DeployClient();
        }

        public void SendMessage(string message)
        {
            try
            {
                if (listenSocket == null)
                {
                    Console.WriteLine("Клиент был закрыт, разверние новый клиент!");
                    return;
                }

                byte[] buffer = Encoding.Unicode.GetBytes(message);
                IPEndPoint remotePoint = new IPEndPoint(Address, RemotePort);
                listenSocket.SendTo(buffer, remotePoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Listen()
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(Address, LocalPort);
                listenSocket!.Bind(localIP);

                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] buffer = new byte[256];
                    EndPoint remoteIP = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listenSocket.ReceiveFrom(buffer, ref remoteIP);
                        builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    } while (listenSocket.Available > 0);

                    IPEndPoint remoteFullIP = remoteIP as IPEndPoint;

                    Console.WriteLine($"{remoteFullIP!.Address}:{remoteFullIP.Port} - {builder}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }

        public void DeployClient()
        {
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Task listeningTask = new Task(Listen);
            listeningTask.Start();
        }

        public void Dispose()
        {
            if (listenSocket != null)
            {
                listenSocket.Shutdown(SocketShutdown.Both);
                listenSocket.Close();
                listenSocket = null;
            }
        }
    }
}
