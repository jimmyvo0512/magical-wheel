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

    public void SetStatus(string status)
    {
        if (this.status == null)
        {
            return;
        }

        this.status.text = status;
    }
}