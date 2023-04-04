using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSceneMgr : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(Application.Quit);
        submitButton.onClick.AddListener(SubmitName);
    }

    public void SetStatus(string status, bool canSubmit)
    {
        statusText.text = status;
        SetCanSubmit(canSubmit);
    }

    private void SubmitName()
    {
        TCPSender.Register(nameInputField.text);
        SetCanSubmit(false);
    }

    private void SetCanSubmit(bool canSubmit)
    {
        nameInputField.interactable = canSubmit;
        submitButton.interactable = canSubmit;
    }
}