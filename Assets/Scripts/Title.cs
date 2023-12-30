using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.ChangeScene("DeadOutTest");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
