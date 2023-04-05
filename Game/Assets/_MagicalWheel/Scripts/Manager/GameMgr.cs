using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameMgr : Singleton<GameMgr>
{
    string playerName;

    public string PlayerName => playerName;

    public void Register(string playerName)
    {
        this.playerName = playerName;
        TCPSender.Register(this.playerName);
    }

    public void Answer(char character, string keyword)
    {
        TCPSender.Answer(character, keyword);
    }

    public void HandleConnecting(bool ok)
    {
    }

    public void HandleRegisterResp(RegisterResp resp)
    {
    }

    public void HandleNewPlayerInform(string playerName)
    {
    }

    public void HandleStartGame(string question, int answerLen, string playerName)
    {
    }

    public void HandlePlayerTurn(int turn, string playerName)
    {
    }

    public void HandleCorrectChar(Dictionary<string, int> scoreBoard, string nextPlayerName)
    {
    }

    public void HandleEndGame(string resultKeyword, Dictionary<string, int> scoreBoard)
    {
    }
}