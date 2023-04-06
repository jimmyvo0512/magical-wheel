using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Register,
    InGame,
    EndGame,
}

public class GameMgr : Singleton<GameMgr>
{
    [SerializeField] GameObject endGame;
    [SerializeField] Button back;

    string playerName;

    public string PlayerName => playerName;

    RegisterSceneMgr registerSceneMgr() { return FindObjectOfType<RegisterSceneMgr>(); }
    GameSceneMgr gameSceneMgr() { return FindObjectOfType<GameSceneMgr>(); }
    RankSceneMgr rankSceneMgr() { return FindObjectOfType<RankSceneMgr>(); }

    List<SceneMgr> sceneMgrs()
    {
        return new List<SceneMgr>() { registerSceneMgr(), gameSceneMgr(), rankSceneMgr() };
    }

    bool inTesting => TestMgr.Instance.InTesting;

    private void Start()
    {
        SetState(GameState.Register);
        back.onClick.AddListener(BackToHome);

        if (inTesting) { HandleConnecting(true); }
    }

    public void Register(string playerName)
    {
        this.playerName = playerName;

        if (inTesting)
        {
            TestMgr.Instance.Register(playerName);
            return;
        }

        TCPSender.Register(this.playerName);
    }

    public void Answer(char character, string keyword)
    {
        if (inTesting)
        {
            TestMgr.Instance.Answer(playerName, character, keyword);
            return;
        }

        TCPSender.Answer(character, keyword);
    }

    public void HandleConnecting(bool connected)
    {
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleConnecting(connected));
    }

    public void HandleRegisterResp(RegisterResp resp)
    {
        if (resp == RegisterResp.OK)
        {
            sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleRegisterResp(true));
            return;
        }

        var err = resp == RegisterResp.Invalid ? "Invalid player name!" : "Player name already exists!";
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleRegisterResp(false, err));
    }

    public void HandleNewPlayerInform(string playerName)
    {
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleNewPlayerInform(playerName));
    }

    public void HandleStartGame(string question, int answerLen, string playerName)
    {
        SetState(GameState.InGame);
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleStartGame(question, answerLen, playerName));
    }

    public void HandlePlayerTurn(int turn, string playerName)
    {
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandlePlayerTurn(turn, playerName));
    }

    public void HandleCorrectChar(string curKeyword, Dictionary<string, int> scoreBoard, string nextPlayerName)
    {
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleCorrectChar(curKeyword, scoreBoard, nextPlayerName));
    }

    public void HandleEndGame(string resultKeyword, Dictionary<string, int> scoreBoard)
    {
        SetState(GameState.EndGame);
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleEndGame(scoreBoard));
    }

    private void BackToHome()
    {
        SetState(GameState.Register);
        sceneMgrs().ForEach(sceneMgr => sceneMgr.HandleGameAlreadyStarted());
    }

    private void SetState(GameState state)
    {
        switch (state)
        {
            case GameState.Register:
                registerSceneMgr().SetMaster(true);
                gameSceneMgr().SetMaster(false);
                rankSceneMgr().SetMaster(false);
                endGame.SetActive(false);
                return;
            case GameState.InGame:
                registerSceneMgr().SetMaster(false);
                gameSceneMgr().SetMaster(true);
                rankSceneMgr().SetMaster(true);
                endGame.SetActive(false);
                return;
            case GameState.EndGame:
                registerSceneMgr().SetMaster(false);
                gameSceneMgr().SetMaster(false);
                rankSceneMgr().SetMaster(true);
                endGame.SetActive(true);
                return;
        }
    }
}