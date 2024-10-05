using System.Net;

namespace SynFlood;

public static class IpUtils
{
    public static List<IPAddress> GetTargetIpAddresses(string destinationIp, bool attackAllInterface)
    {
        var ipAddresses = new List<IPAddress>();
        if (attackAllInterface)
        {
            var hostEntry = Dns.GetHostEntry(destinationIp);
            ipAddresses.AddRange(hostEntry.AddressList);
        }

        var destinationIpAddress = IPAddress.Parse(destinationIp);
        if (!ipAddresses.Any(ip => ip.Equals(destinationIpAddress)))
        {
            ipAddresses.Add(destinationIpAddress);
        }

        return ipAddresses;
    }

    public static (string sourceIp, short sourcePort, short targetPort) GetRandomAndIpPortData(List<short> openPorts)
    {
        var sourceIp = GetRandomIp();
        var sourcePort = GetRandomSourcePort();
        var targetPort = GetRandomTargetPort(openPorts);
        return (sourceIp, sourcePort, targetPort);
    }

    private static int GetRandomByte()
    {
        return Random.Shared.Next(1, Byte.MaxValue);
    }

    private static string GetRandomIp()
    {
        return $"{GetRandomByte()}.{GetRandomByte()}.{GetRandomByte()}.{GetRandomByte()}";
    }

    public static short GetRandomSourcePort()
    {
        return (short)Random.Shared.Next(1, Int16.MaxValue);
    }

    private static short GetRandomTargetPort(List<short> openPorts)
    {
        if (openPorts.Count == 1)
        {
            return openPorts[0];
        }

        var portIndex = Random.Shared.Next(0, openPorts.Count);
        return openPorts[portIndex];
    }
}
