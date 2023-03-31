using System;
using UnityEngine;

public enum ServerType
{
    ResponseRegistration = 0,
    InformNewPlayer,
    SendGameQuestion,
    Turn,
    EndGame,
}

public class TCPHandler
{
    public static void HandleData(byte[] data)
    {
        try
        {
            var decoder = new TCPDecoder(data);

            var svType = (ServerType)decoder.GetInt();
            Debug.Log("ServerType: " + svType.ToString());

            switch (svType)
            {
                case ServerType.ResponseRegistration:
                    ResponseRegistration(decoder);
                    return;
                case ServerType.InformNewPlayer:
                    InformNewPlayer(decoder);
                    return;
                case ServerType.SendGameQuestion:
                    SendGameQuestion(decoder);
                    return;
                case ServerType.Turn:
                    Turn(decoder);
                    return;
                case ServerType.EndGame:
                    EndGame(decoder);
                    return;
                default:
                    throw new Exception("Unexpected server type: " + svType.ToString());
            }
        }
        catch (Exception err)
        {
            Debug.LogError("HandleData Err: " + err.Message);
        }
    }

    private static void ResponseRegistration(TCPDecoder decoder)
    {
        var ok = decoder.GetBool();
        var playerOrErr = decoder.GetString();
        GameMgr.Instance.ResponseRegistration(ok, playerOrErr);
    }

    private static void InformNewPlayer(TCPDecoder decoder)
    {
        var player = decoder.GetString();
        GameMgr.Instance.InformNewPlayer(player);
    }

    private static void SendGameQuestion(TCPDecoder decoder)
    {
        var question = decoder.GetString();
        var answerLen = decoder.GetInt();
        GameMgr.Instance.SendGameQuestion(question, answerLen);
    }

    private static void Turn(TCPDecoder decoder)
    {
        var player = decoder.GetString();
        GameMgr.Instance.Turn(player);
    }

    private static void EndGame(TCPDecoder decoder)
    {

    }
}