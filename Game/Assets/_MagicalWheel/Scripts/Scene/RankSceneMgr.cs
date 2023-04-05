using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSceneMgr : SceneMgr
{
    [SerializeField] PlayerRank playerRankPrefab;

    private void Awake()
    {
        Set(new Dictionary<string, int>());
    }

    public override void HandleCorrectChar(string curKeyword, Dictionary<string, int> scoreBoard, string nextPlayerName) => Set(scoreBoard);
    public override void HandleEndGame(Dictionary<string, int> scoreBoard) => Set(scoreBoard);

    private void Set(Dictionary<string, int> scoreBoard)
    {
        foreach (Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        var rankedPlayers = scoreBoard.Keys.OrderBy(player => scoreBoard[player]).Reverse().ToList();
        for (var i = 0; i < rankedPlayers.Count; i++)
        {
            var rank = Instantiate(playerRankPrefab, Container.transform);
            rank.Set(i < 3 ? i + 1 : -1, rankedPlayers[i], scoreBoard[rankedPlayers[i]]);
        }
    }
}