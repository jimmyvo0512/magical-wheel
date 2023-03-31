using System;
using System.Collections.Generic;
using System.Text;

public class TCPEncoder
{
    List<byte> buffer;

    public TCPEncoder()
    {
        buffer = new List<byte>();
    }

    public byte[] ToArray() => buffer.ToArray();

    public void AddInt(int n) => buffer.AddRange(BitConverter.GetBytes(n));
    public void AddFloat(float f) => buffer.AddRange(BitConverter.GetBytes(f));
    public void AddBool(bool b) => buffer.AddRange(BitConverter.GetBytes(b));
    public void AddString(string str)
    {
        AddInt(str.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(str));
    }
}