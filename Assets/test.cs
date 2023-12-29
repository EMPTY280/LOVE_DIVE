using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.ChangeScene("Scene2");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.ChangeScene("SampleScene");
        }
    }
}