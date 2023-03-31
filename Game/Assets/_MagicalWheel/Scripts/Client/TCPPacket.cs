using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ServerPacketEnum
{
    none,
}

public enum ClientPacketEnum
{
    none,
}

public class TCPPacketEncoder
{
    List<byte> buffer;

    public void ToArray() => buffer.ToArray();

    public void AddInt(int n) => buffer.AddRange(BitConverter.GetBytes(n));
    public void AddFloat(float f) => buffer.AddRange(BitConverter.GetBytes(f));
    public void AddBool(bool b) => buffer.AddRange(BitConverter.GetBytes(b));
    public void AddString(string str)
    {
        AddInt(str.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(str));
    }
}

public class TCPPacketDecoder
{
    byte[] buffer;
    int seekPos;

    public TCPPacketDecoder(byte[] data)
    {
        buffer = data;
        seekPos = 0;
    }

    public int GetInt()
    {
        try
        {
            var res = BitConverter.ToInt32(buffer, seekPos);
            seekPos += sizeof(int);
            return res;
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            throw new Exception("Cannot read buffer!");
        }
    }

    public float GetFloat()
    {
        try
        {
            var res = BitConverter.ToSingle(buffer, seekPos);
            seekPos += sizeof(float);
            return res;
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            throw new Exception("Cannot read buffer!");
        }
    }

    public bool GetBool()
    {
        try
        {
            var res = BitConverter.ToBoolean(buffer, seekPos);
            seekPos += sizeof(bool);
            return res;
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            throw new Exception("Cannot read buffer!");
        }
    }

    public string GetString()
    {
        try
        {
            var len = GetInt();
            var res = Encoding.ASCII.GetString(buffer, seekPos, len);
            seekPos += len;
            return res;
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            throw new Exception("Cannot read buffer!");
        }
    }
}