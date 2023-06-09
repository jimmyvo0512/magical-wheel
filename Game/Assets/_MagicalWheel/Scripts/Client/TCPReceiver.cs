using System;
using UnityEngine;

public enum ServerType
{
    RegisterResp = 1,
    NewPlayerInform,
    StartGame,
    PlayerTurn,
    CorrectChar,
    EndGame,
}

public enum RegisterResp
{
    OK = 1,
    Invalid,
    AlreadyExist,
}

public class TCPReceiver
{
    public static void HandleData(byte[] data)
    {
        try
        {
            var decoder = new TCPDecoder(data);

            var svType = (ServerType)decoder.GetInt8();
            Debug.Log("ServerType: " + svType.ToString());

            switch (svType)
            {
                case ServerType.RegisterResp:
                    ReceiveRegisterResp(decoder);
                    return;
                case ServerType.NewPlayerInform:
                    ReceiveNewPlayerInform(decoder);
                    return;
                case ServerType.StartGame:
                    ReceiveStartGame(decoder);
                    return;
                case ServerType.PlayerTurn:
                    ReceivePlayerTurn(decoder);
                    return;
                case ServerType.CorrectChar:
                    ReceiveCorrectChar(decoder);
                    return;
                case ServerType.EndGame:
                    ReceiveEndGame(decoder);
                    return;
                default:
                    throw new Exception("Unexpected server type: " + svType.ToString());
            }
        }
        catch (Exception err)
        {
            Debug.LogError("HandleData Err: " + err.Message);
        }
        finally
        {
            TCPMgr.Instance.UnlockMessageQueue();
        }
    }

    private static void ReceiveRegisterResp(TCPDecoder decoder)
    {
        var resp = (RegisterResp)decoder.GetInt8();

        TCPSender.Log(new { resp });

        GameMgr.Instance.HandleRegisterResp(resp);
    }

    private static void ReceiveNewPlayerInform(TCPDecoder decoder)
    {
        var playerName = decoder.GetString();

        TCPSender.Log(new { playerName });

        GameMgr.Instance.HandleNewPlayerInform(playerName);
    }

    private static void ReceiveStartGame(TCPDecoder decoder)
    {
        var turnId = decoder.GetInt();
        var answerLen = decoder.GetInt();
        var question = decoder.GetString();
        var playerName = decoder.GetString();

        TCPSender.Log(new { turnId, answerLen, question, playerName });

        GameMgr.Instance.HandleStartGame(question, answerLen, playerName);
    }

    private static void ReceivePlayerTurn(TCPDecoder decoder)
    {
        var turn = decoder.GetInt();
        var resKeyword = decoder.GetString();
        var playerName = decoder.GetString();

        TCPSender.Log(new { turn, resKeyword, playerName });

        GameMgr.Instance.HandlePlayerTurn(turn, playerName);
    }

    private static void ReceiveCorrectChar(TCPDecoder decoder)
    {
        var turnId = decoder.GetInt();
        var resKeyword = decoder.GetString();
        var scoreBoard = decoder.GetScoreBoard();
        var playerName = decoder.GetString();

        TCPSender.Log(new { turnId, resKeyword, scoreBoard, playerName });

        GameMgr.Instance.HandleCorrectChar(resKeyword, scoreBoard, playerName);
    }

    private static void ReceiveEndGame(TCPDecoder decoder)
    {
        var resKeyword = decoder.GetString();
        var scoreBoard = decoder.GetScoreBoard();

        TCPSender.Log(new { resKeyword, scoreBoard });

        GameMgr.Instance.HandleEndGame(resKeyword, scoreBoard);
    }
}