using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInput : IInput
{
    public float GetHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVertical()
    {
        return Input.GetAxis("Vertical");
    }
}
