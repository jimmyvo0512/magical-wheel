using TMPro;
using UnityEngine;

public class PlayerRank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rank, playerName, score;

    public void Set(int rank, string playerName, int score)
    {
        this.playerName.text = playerName;
        if (this.playerName.text == GameMgr.Instance.PlayerName)
        {
            this.playerName.text += " (You)";
        }

        this.rank.text = GetRank(rank);
        this.score.text = score.ToString();
    }

    private string GetRank(int rank)
    {
        if (rank <= 0)
        {
            return string.Empty;
        }

        switch (rank)
        {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return rank.ToString() + "th";
        }
    }
}