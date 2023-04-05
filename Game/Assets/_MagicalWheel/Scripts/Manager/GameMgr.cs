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

    RegisterSceneMgr registerSceneMgr => FindObjectOfType<RegisterSceneMgr>();
    GameSceneMgr gameSceneMgr => FindObjectOfType<GameSceneMgr>();
    RankSceneMgr rankSceneMgr => FindObjectOfType<RankSceneMgr>();

    List<SceneMgr> sceneMgrs => new List<SceneMgr>() { registerSceneMgr, gameSceneMgr, rankSceneMgr };

    protected override void Awake()
    {
        base.Awake();

        SetState(GameState.Register);
        back.onClick.AddListener(() => SetState(GameState.Register));
    }

    public void Register(string playerName)
    {
        this.playerName = playerName;
        TCPSender.Register(this.playerName);
    }

    public void Answer(char character, string keyword)
    {
        TCPSender.Answer(character, keyword);
    }

    public void HandleConnecting(bool connected)
    {
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleConnecting(connected));
    }

    public void HandleRegisterResp(RegisterResp resp)
    {
        if (resp == RegisterResp.OK)
        {
            sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleRegisterResp(true));
        }

        var err = resp == RegisterResp.Invalid ? "Invalid player name!" : "Player name already exists!";
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleRegisterResp(false, err));
    }

    public void HandleNewPlayerInform(string playerName)
    {
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleNewPlayerInform(playerName));
    }

    public void HandleStartGame(string question, int answerLen, string playerName)
    {
        SetState(GameState.InGame);
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleStartGame(question, answerLen, playerName));
    }

    public void HandlePlayerTurn(int turn, string playerName)
    {
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandlePlayerTurn(turn, playerName));
    }

    public void HandleCorrectChar(Dictionary<string, int> scoreBoard, string nextPlayerName)
    {
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleCorrectChar(scoreBoard, nextPlayerName));
    }

    public void HandleEndGame(string resultKeyword, Dictionary<string, int> scoreBoard)
    {
        SetState(GameState.EndGame);
        sceneMgrs.ForEach(sceneMgr => sceneMgr.HandleEndGame(scoreBoard));
    }

    private void SetState(GameState state)
    {
        switch (state)
        {
            case GameState.Register:
                registerSceneMgr.SetMaster(true);
                gameSceneMgr.SetMaster(false);
                rankSceneMgr.SetMaster(false);
                endGame.SetActive(false);
                return;
            case GameState.InGame:
                registerSceneMgr.SetMaster(false);
                gameSceneMgr.SetMaster(true);
                rankSceneMgr.SetMaster(true);
                endGame.SetActive(false);
                return;
            case GameState.EndGame:
                registerSceneMgr.SetMaster(false);
                gameSceneMgr.SetMaster(false);
                rankSceneMgr.SetMaster(true);
                endGame.SetActive(true);
                return;
        }
    }
}