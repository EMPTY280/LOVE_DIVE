using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] GameObject credit = null;

    public void StartGame()
    {
        GameManager.Instance.ChangeScene("DeadOutTest");
    }

    public void OpenCredit()
    {
        credit.SetActive(true);
    }

    public void CloseCredit()
    {
        credit.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
