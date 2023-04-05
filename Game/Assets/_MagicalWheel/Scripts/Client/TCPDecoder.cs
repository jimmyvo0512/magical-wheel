using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TCPDecoder
{
    byte[] buffer;
    int seekPos;

    public TCPDecoder(byte[] data)
    {
        Debug.Log(data.Length);
        buffer = data;
        seekPos = 0;
    }

    public Dictionary<string, int> GetScoreBoard()
    {
        var playerCnt = GetInt();

        var res = new Dictionary<string, int>();
        for (var i = 0; i < playerCnt; i++)
        {
            var playerName = GetString();
            var score = GetInt();

            res[playerName] = score;
        }

        return res;
    }

    public int GetInt() => BitConverter.ToInt32(buffer, MoveSeekPos(sizeof(int)));
    public sbyte GetInt8() => (sbyte)buffer[MoveSeekPos(sizeof(sbyte))];
    public float GetFloat() => BitConverter.ToSingle(buffer, MoveSeekPos(sizeof(float)));
    public bool GetBool() => BitConverter.ToBoolean(buffer, MoveSeekPos(sizeof(bool)));
    public string GetString()
    {
        var len = GetInt();
        return Encoding.ASCII.GetString(buffer, MoveSeekPos(len), len);
    }

    public int MoveSeekPos(int size)
    {
        seekPos += size;
        return seekPos - size;
    }
}