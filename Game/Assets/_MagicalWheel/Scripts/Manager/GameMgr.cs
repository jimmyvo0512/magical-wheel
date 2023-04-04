using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameMgr : Singleton<GameMgr>
{
    string playerName;

    public void Register(string playerName)
    {
        this.playerName = playerName;
        TCPSender.Register(this.playerName);
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