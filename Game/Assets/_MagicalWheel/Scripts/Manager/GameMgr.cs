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

    public void Connect(bool ok)
    {
        var status = ok ? "Connected to server!" : "Can't find server!";
        FindObjectOfType<RegisterSceneMgr>().SetStatus(status, ok);
    }

    public void HandleRegisterResp(RegisterResp resp)
    {
        switch (resp)
        {
            case RegisterResp.OK:
            case RegisterResp.Invalid:
            case RegisterResp.AlreadyExist:
            default:
                break;
        }
    }

    public void HandleNewPlayerInform(string playerName)
    {
        var status = "Player " + playerName + " just joined!";
        FindObjectOfType<RegisterSceneMgr>().SetStatus(status, false);
    }

    public void HandleStartGame(string question, int answerLen, string playerName)
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            SceneManager.LoadSceneAsync("Game").completed += async asyncOpr =>
            {
                await Task.Delay(500);
                FindObjectOfType<GameSceneMgr>().SetQuestion(question, answerLen);
            };

            return;
        }

        FindObjectOfType<GameSceneMgr>().SetQuestion(question, answerLen);
    }

    public void HandlePlayerTurn(int turn, string playerName)
    {
        FindObjectOfType<GameSceneMgr>().SetTurn(playerName == this.playerName);
    }

    public void HandleCorrectChar(Dictionary<string, int> scoreBoard, string nextPlayerName)
    {

    }

    public void HandleEndGame(string resultKeyword, Dictionary<string, int> scoreBoard)
    {
        if (SceneManager.GetActiveScene().name != "Rank")
        {
            SceneManager.LoadSceneAsync("Rank").completed += async asyncOpr =>
            {
                await Task.Delay(500);
            };
        }
    }
}