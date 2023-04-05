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

    protected void SetStatus(string status)
    {
        if (this.status == null)
        {
            return;
        }

        this.status.text = status;
    }
}