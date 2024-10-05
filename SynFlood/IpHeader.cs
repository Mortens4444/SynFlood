using System.Runtime.InteropServices;

namespace SynFlood;

[StructLayout(LayoutKind.Sequential)]
public struct IpHeader
{
    [BitfieldLength(4)]
    public uint ihl;

    [BitfieldLength(4)]
    public uint version;

    public byte tos;

    public uint tot_len;

    public long id;

    public uint frag_off;

    public byte ttl;

    public byte protocol;

    public uint check;

    public uint saddr;

    public uint daddr;
}