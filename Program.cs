using UDP_Simple_App;

Console.WriteLine("***** UDP Application *****");

int localPort = 0;
int remotePort = 0;
string ipAddress = "127.0.0.1";

try
{
    Console.WriteLine("\nВведите порт для приема сообщений:");
    localPort = int.Parse(Console.ReadLine());
    Console.WriteLine("\nВведите порт для отправки сообщений:");
    remotePort = int.Parse(Console.ReadLine());
    Console.WriteLine("\nВведите ваше сообщение:\n");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

using (UDP_Client client = new UDP_Client(localPort, remotePort, ipAddress))
{
    try
    {
        string message = string.Empty;

        while (true)
        {
            message = Console.ReadLine();
            client.SendMessage(message);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}