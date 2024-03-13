using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    public static async Task SaveMsg(EndPoint adres, string msg)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
        string newIp = ModIP(adres, ipPoint);
        string[] data = msg.Split(":");
        string fileName = GenerateFilenAME(adres.ToString(), data[0]);
        using var sw = new StreamWriter(String.Format($"C:/Users/is22-11/source/repos/anotherserv/anotherserv/bin/Debug/net8.0/msgs/{newIp}.txt"), true);
        await sw.WriteAsync(adres.ToString() + '_' + msg + "\n");
        sw.Flush();
    }
    public static string GenerateFilenAME(string firstIp, string secondIp)
    {
        for (int i = 0; i < firstIp.Length; i++)
        {
            int.TryParse(firstIp.Substring(i, 1), out int val);
            int.TryParse(secondIp.Substring(i, 1), out int val2);

            if (val > val2)
            {
                return ModIP(secondIp) + "_" + ModIP(firstIp);
            }
            else if (val2 > val)
            {
                return ModIP(firstIp) + "_" + ModIP(secondIp);
            }
        }
        return null;
    }
    public static string ModIP(EndPoint adres, IPEndPoint ipPoint)
    {
        return adres.ToString().Replace('.', '_').Substring(0, 15);

    }
    public static string ModIP(string adres)
    {
        return adres.ToString().Replace('.', '_').Substring(0, 15);

    }
    public static async Task Parse(string msg)
    {
    }
    public static async Task Main()
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 12345);
        socket.Bind(ipPoint);
        socket.Listen();
        while (true)
        {
            using var tcpClient = await socket.AcceptAsync();
            System.Console.WriteLine("Есть подключиние");
            byte[] bytesRead = new byte[255];
            int count = await tcpClient.ReceiveAsync(bytesRead, SocketFlags.None);
            string msg = Encoding.UTF8.GetString(bytesRead);
            await Parse(msg);
            await Console.Out.WriteLineAsync("Принято сообщение");

            if (count > 0)
            {
                await SaveMsg(tcpClient.RemoteEndPoint, msg);
            }
        }
    }
}
