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
    }
}