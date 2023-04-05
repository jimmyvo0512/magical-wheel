using Newtonsoft.Json;
using UnityEngine;

public enum ClientType
{
    Register = 1,
    Answer,
}

public class TCPSender
{
    public static void Register(string playerName)
    {
        var encoder = new TCPEncoder();
        encoder.AddInt8((sbyte)ClientType.Register);
        encoder.AddString(playerName);

        Log(new { type = ClientType.Register, playerName, });

        Send(encoder);
    }

    public static void Answer(char character, string keyword)
    {
        var encoder = new TCPEncoder();
        encoder.AddInt8((sbyte)ClientType.Answer);
        encoder.AddChar(character);
        encoder.AddString(keyword);

        Log(new { type = ClientType.Answer, character, keyword, });

        Send(encoder);
    }

    private static void Send(TCPEncoder encoder)
    {
        var buffer = encoder.ToArray();
        TCPEncoder.Log(buffer);

        TCPMgr.Instance.Send(buffer);
    }

    public static void Log(object obj)
    {
        Debug.Log(JsonConvert.SerializeObject(obj));
    }
}