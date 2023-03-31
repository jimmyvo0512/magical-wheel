using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMgr : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText, hintText, scoreText, statusText;
    [SerializeField] TMP_InputField answerInputField;
    [SerializeField] Button submitButton, quitButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(Application.Quit);
    }
}