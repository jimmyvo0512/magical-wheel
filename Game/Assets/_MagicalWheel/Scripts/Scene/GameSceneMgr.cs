using System.Collections.Generic;
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

    public override void HandleStartGame(string question, int answerLen, string playerName)
    {
        base.HandleStartGame(question, answerLen, playerName);

        this.question.text = question;
        resultKeyword.text = new string('_', answerLen);
    }

    public override void HandlePlayerTurn(int turnId, string playerName)
    {
        base.HandlePlayerTurn(turnId, playerName);

        turn.text = "Turn " + turnId.ToString() + ": " + playerName + " is in turn!";

        inTurn = playerName == GameMgr.Instance.PlayerName;
        character.interactable = keyword.interactable = submit.interactable = inTurn;
    }

    public override void HandleCorrectChar(string curKeyword, Dictionary<string, int> scoreBoard, string nextPlayerName)
    {
        base.HandleCorrectChar(curKeyword, scoreBoard, nextPlayerName);

        resultKeyword.text = curKeyword;
        HandlePlayerTurn(0, nextPlayerName);
    }

    private void Submit()
    {
        GameMgr.Instance.Answer(character.text[0], keyword.text);
    }
}