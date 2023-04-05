using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestMgr : Singleton<TestMgr>
{
    public bool InTesting => true;

    const string QUESTION = "What is a socket address?";
    const string ANSWER = "Tuple";

    static readonly char EMPTY = "_"[0];

    string curKeyword;
    string curPlayer;

    List<string> players = new List<string>() { "jimmy", "alex", "john" };
    Dictionary<string, int> playersDict = new Dictionary<string, int>();

    private void Start()
    {
        curKeyword = new string(EMPTY, ANSWER.Length);

        players.ForEach(player => playersDict[player] = 0);
    }

    public async void Register(string playerName)
    {
        foreach (var c in playerName)
        {
            if (!char.IsLetterOrDigit(c))
            {
                GameMgr.Instance.HandleRegisterResp(RegisterResp.Invalid);
                return;
            }
        }

        players.Add(playerName);
        playersDict[playerName] = 0;

        GameMgr.Instance.HandleRegisterResp(RegisterResp.OK);

        await Task.Delay(3000);

        GameMgr.Instance.HandleStartGame(QUESTION, ANSWER.Length, players[0]);
        OtherPlayersTurns();
    }

    public void Answer(string curPlayer, char character, string keyword)
    {
        this.curPlayer = curPlayer;
        CurPlayerAnswer(character, keyword);
    }

    private void CurPlayerAnswer(char character, string keyword)
    {
        if (keyword == ANSWER)
        {
            playersDict[curPlayer] += 5;
            GameMgr.Instance.HandleEndGame(curKeyword, playersDict);
        }

        if (GetRemains().Contains(character))
        {
            for (var i = 0; i < ANSWER.Length; i++)
            {
                if (curKeyword[i] == EMPTY && ANSWER[i] == character)
                {
                    var cur = curKeyword.ToCharArray();
                    cur[i] = character;
                    curKeyword = new string(cur);

                    playersDict[curPlayer] += 1;

                    GameMgr.Instance.HandleCorrectChar(playersDict, GetNextPlayer());
                    if (curPlayer == GameMgr.Instance.PlayerName)
                    {
                        OtherPlayersTurns();
                    }

                    return;
                }
            }
        }

        GameMgr.Instance.HandlePlayerTurn(0, GetNextPlayer());
        if (curPlayer == GameMgr.Instance.PlayerName)
        {
            OtherPlayersTurns();
        }
    }

    private async void OtherPlayersTurns()
    {
        for (var i = 0; i < 3; i++)
        {
            curPlayer = players[i];
            await Task.Delay(3000);

            Answer(players[i], GetRemains()[0], string.Empty);
        }
    }

    private List<char> GetRemains()
    {
        var remains = new List<char>();
        for (var i = 0; i < ANSWER.Length; i++)
        {
            if (curKeyword[i] == EMPTY)
            {
                remains.Add(ANSWER[i]);
            }
        }

        return remains;
    }

    private string GetNextPlayer()
    {
        var next = players.IndexOf(curKeyword) + 1;
        return players[next >= players.Count ? 0 : next];
    }
}