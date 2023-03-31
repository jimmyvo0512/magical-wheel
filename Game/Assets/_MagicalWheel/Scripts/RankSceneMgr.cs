using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSceneMgr : MonoBehaviour
{
    Canvas canvas;
    Button quitButton;
    List<TextMeshProUGUI> rankLs;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();

        quitButton = canvas.transform.GetChild(0).GetComponent<Button>();
        quitButton.onClick.AddListener(Application.Quit);

        rankLs = new List<TextMeshProUGUI>();
        for (var i = 0; i < canvas.transform.childCount; i++)
        {
            rankLs.Add(canvas.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }
}