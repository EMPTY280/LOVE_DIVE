using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    private void Awake()
    {
        foreach (Button item in buttons)
        {
            switch (item.name)
            {
                case "StartBtn":
                    item.onClick.AddListener(StartGame);
                    break;
                case "ExitBtn":
                    item.onClick.AddListener(ExitGame);
                    break;
                default:
                    break;
            }
        }
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeScene("DeadOutTest");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
