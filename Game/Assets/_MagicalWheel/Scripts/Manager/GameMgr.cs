using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameMgr : Singleton<GameMgr>
{
    string playerName;

    public void Connect(bool ok)
    {
        var status = ok ? "Connected to server!" : "Can't find server!";
        FindObjectOfType<RegisterSceneMgr>().SetStatus(status, ok);
    }

    public void ResponseRegistration(bool ok, string playerNameOrErr)
    {
        if (ok)
        {
            playerName = playerNameOrErr;
        }

        var status = ok ? "Register done! Pls wait for other players..." : playerNameOrErr;
        FindObjectOfType<RegisterSceneMgr>().SetStatus(status, !ok);
    }

    public void InformNewPlayer(string playerName)
    {
        var status = "Player " + playerName + " just joined!";
        FindObjectOfType<RegisterSceneMgr>().SetStatus(status, false);
    }

    public void SendGameQuestion(string question, int answerLen)
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

    public void Turn(string playerName)
    {
        FindObjectOfType<GameSceneMgr>().SetTurn(playerName == this.playerName);
    }

    public void EndGame()
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