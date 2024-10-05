using System.Net;

namespace SynFlood;

public static class TcpHeaderBuilder
{
    public static TcpHeader Forge(short destinationPort, short sourcePort, short maximumAllowedWindowSize = 5840)
    {
        return new TcpHeader
        {
            source = IPAddress.HostToNetworkOrder(sourcePort),
            dest = IPAddress.HostToNetworkOrder(destinationPort),
            seq = 0,
            ack_seq = 0,
            doff = 5,
            fin = 0,
            syn = 1,
            rst = 0,
            psh = 0,
            ack = 0,
            urg = 0,
            window = IPAddress.HostToNetworkOrder(maximumAllowedWindowSize),
            check = 0,
            urg_ptr = 0
        };
    }
}
