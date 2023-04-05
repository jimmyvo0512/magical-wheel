using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSceneMgr : SceneMgr
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button submit;

    private void Awake()
    {
        submit.onClick.AddListener(Register);
        playerName.interactable = submit.interactable = false;
    }

    public override void HandleConnecting(bool connected)
    {
        base.HandleConnecting(connected);
        playerName.interactable = submit.interactable = connected;
    }

    public override void HandleRegisterResp(bool ok, string err = null)
    {
        base.HandleRegisterResp(ok, err);
        if (ok)
        {
            playerName.interactable = false;
            submit.interactable = false;
        }
    }

    private void Register()
    {
        GameMgr.Instance.Register(playerName.text);
    }
}