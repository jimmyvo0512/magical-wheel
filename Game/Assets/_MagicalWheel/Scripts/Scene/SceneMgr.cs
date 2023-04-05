using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    [SerializeField] GameObject master, container;
    [SerializeField] TextMeshProUGUI status;

    public GameObject Container => container;

    public void SetMaster(bool enable)
    {
        master.SetActive(enable);
    }

    public virtual void HandleConnecting(bool connected)
    {
        var status = connected ? "Connected to server!" : "Cannot connect to server!";
        SetStatus(status);
    }

    public virtual void HandleRegisterResp(bool ok, string err = null)
    {
        var status = ok ? "Register done! Please wait for other players" : ("Failed to register: " + err + "!");
        SetStatus(status);
    }

    public virtual void HandleNewPlayerInform(string playerName)
    {
        SetStatus(playerName + " just joined!");
    }

    public virtual void HandleStartGame(string question, int answerLen, string playerName) { }

    public virtual void HandlePlayerTurn(int turnId, string playerName) { }

    public virtual void HandleCorrectChar(string curKeyword, Dictionary<string, int> scoreBoard, string nextPlayerName) { }

    public virtual void HandleEndGame(Dictionary<string, int> scoreBoard) { }

    protected void SetStatus(string status)
    {
        if (this.status == null)
        {
            return;
        }

        this.status.text = status;
    }
}