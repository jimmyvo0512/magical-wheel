using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMgr : SceneMgr
{
    [SerializeField] TextMeshProUGUI question, resultKeyword, turn;
    [SerializeField] TMP_InputField character, keyword;
    [SerializeField] Button submit;

    bool inTurn = false;

    private void Awake()
    {
        submit.onClick.AddListener(Submit);
    }

    public static GameSceneMgr Get()
    {
        return FindObjectOfType<GameSceneMgr>();
    }

    public void HandleStartGame(string question, int answerLen, string playerName)
    {
        this.question.text = question;
        resultKeyword.text = new string('_', answerLen);
    }

    public void HandlePlayerTurn(int turnId, string playerName)
    {
        turn.text = "Turn " + turnId.ToString() + ": " + playerName + " is in turn!";

        inTurn = playerName == GameMgr.Instance.PlayerName;
        character.interactable = keyword.interactable = submit.interactable = inTurn;
    }

    public void HandleCorrectChar(string playerName)
    {
        HandlePlayerTurn(0, playerName);
    }

    private void Submit()
    {
        GameMgr.Instance.Answer(character.text[0], keyword.text);
    }
}