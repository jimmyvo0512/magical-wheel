using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMgr : SceneMgr
{
    [SerializeField] TextMeshProUGUI question, resultKeyword, turn;
    [SerializeField] TMP_InputField character, keyword;
    [SerializeField] Button submit;

    bool inTurn => playerInTurn == GameMgr.Instance.PlayerName;
    string playerInTurn;

    private void Awake()
    {
        submit.onClick.AddListener(Submit);
    }

    private void Start()
    {
        SetStatus(string.Empty);
    }

    public override void HandleStartGame(string question, int answerLen, string playerName)
    {
        base.HandleStartGame(question, answerLen, playerName);

        this.question.text = question;
        resultKeyword.text = new string('_', answerLen);

        TurnPlayer(0, playerName);
    }

    public override void HandlePlayerTurn(int turnId, string playerName)
    {
        base.HandlePlayerTurn(turnId, playerName);

        SetStatus(playerInTurn + "'s answer is incorrect!");
        TurnPlayer(turnId, playerName);
    }

    private void TurnPlayer(int turnId, string playerName)
    {
        turn.text = "Turn " + turnId.ToString() + ": " + playerName + " is in turn!";

        playerInTurn = playerName;
        character.interactable = keyword.interactable = submit.interactable = inTurn;
    }

    public override void HandleCorrectChar(string curKeyword, Dictionary<string, int> scoreBoard, string nextPlayerName)
    {
        base.HandleCorrectChar(curKeyword, scoreBoard, nextPlayerName);

        resultKeyword.text = curKeyword;
        SetStatus(playerInTurn + "'s answer is correct!");

        TurnPlayer(0, nextPlayerName);
    }

    private void Submit()
    {
        GameMgr.Instance.Answer(character.text[0], keyword.text);
    }
}