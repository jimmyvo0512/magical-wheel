using System;
using System.Text;

public class TCPDecoder
{
    byte[] buffer;
    int seekPos;

    public TCPDecoder(byte[] data)
    {
        buffer = data;
        seekPos = 0;
    }

    public int GetInt()
    {
        var res = BitConverter.ToInt32(buffer, seekPos);
        seekPos += sizeof(int);
        return res;
    }

    public float GetFloat()
    {
        var res = BitConverter.ToSingle(buffer, seekPos);
        seekPos += sizeof(float);
        return res;
    }

    public bool GetBool()
    {
        var res = BitConverter.ToBoolean(buffer, seekPos);
        seekPos += sizeof(bool);
        return res;
    }

    public string GetString()
    {
        var len = GetInt();
        var res = Encoding.ASCII.GetString(buffer, seekPos, len);
        seekPos += len;
        return res;
    }
}