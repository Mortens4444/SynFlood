using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SynFlood;

public static class PackageBuilder
{
    public static byte[] ConvertToByteArray(ref IpHeader ipHdr, ref TcpHeader tcpHdr)
    {
        var ipHeaderSize = Unsafe.SizeOf<IpHeader>();
        var tcpHeaderSize = Unsafe.SizeOf<TcpHeader>();
        var totalSize = ipHeaderSize + tcpHeaderSize;

        var result = new byte[totalSize];
        var span = new Span<byte>(result);

        var ipHeaderSpan = span[..ipHeaderSize];
        MemoryMarshal.Write(ipHeaderSpan, in ipHdr);

        var tcpHeaderSpan = span.Slice(ipHeaderSize, tcpHeaderSize);
        MemoryMarshal.Write(tcpHeaderSpan, in tcpHdr);

        return result;
    }
}
