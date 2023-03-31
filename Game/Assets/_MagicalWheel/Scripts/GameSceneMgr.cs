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

    public void SetQuestion(string question, int answerLen)
    {
        questionText.text = question;

        hintText.text = string.Empty;
        for (var i = 0; i < answerLen; i++)
        {
            hintText.text += '*';
        }
    }

    public void SetTurn(bool canPlay)
    {
        answerInputField.interactable = canPlay;
        submitButton.interactable = canPlay;
    }
}