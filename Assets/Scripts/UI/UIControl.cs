using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] private Image progressBar = null;
    [SerializeField] private RectTransform hpIcon = null;
    private float hpIconWidth = 0f;

    private static UIControl instance = null;
    public static UIControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("UI").GetComponent<UIControl>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (progressBar == null || hpIcon == null) return;
        hpIconWidth = hpIcon.sizeDelta.x;
    }

    /// <summary>
    /// ���α׷��� �ٸ� ������Ʈ�մϴ�. 
    /// </summary>
    /// <param name="value">���α׷��� ���� ä���� �����Դϴ�. 0.0f ~ 1.0f</param>
    public void UpdateProgressBar(float value)
    {
        progressBar.fillAmount = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    /// hp ǥ�ñ⸦ ������Ʈ�մϴ�.
    /// </summary>
    /// <param name="hp">������Ʈ�� ü���� ���Դϴ�. 0 �̻�</param>
    public void UpdateHP(int hp)
    {
        if (hp < 0) return;
        Vector2 newSize = hpIcon.sizeDelta;
        newSize.x = hp * hpIconWidth;
        hpIcon.sizeDelta = newSize;
    }
}
