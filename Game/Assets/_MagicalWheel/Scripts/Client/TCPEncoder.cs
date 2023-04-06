using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class TCPEncoder
{
    List<byte> buffer;

    public TCPEncoder()
    {
        buffer = new List<byte>();
    }

    public byte[] ToArray() => buffer.ToArray();

    public void AddInt(int n) => buffer.AddRange(BitConverter.GetBytes(n));
    public void AddInt8(sbyte n) => buffer.Add((byte)n);
    public void AddFloat(float f) => buffer.AddRange(BitConverter.GetBytes(f));
    public void AddBool(bool b) => buffer.AddRange(BitConverter.GetBytes(b));
    public void AddChar(char c) => buffer.Add(BitConverter.GetBytes(c)[0]);
    public void AddString(string str)
    {
        AddInt(str.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(str));
    }

    public static void Log(byte[] buffer)
    {
        Debug.Log(JsonConvert.SerializeObject(buffer.Select(bt => bt.ToString("X2")).ToArray()));
    }
}