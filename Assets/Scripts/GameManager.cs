using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Image fadeImage = null;
    private float fadeTime = 1.0f;
    private float transitionDelay = 2.0f;

    private delegate void Callback();
    [SerializeField] private bool isFading = false;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject().AddComponent<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject prefab = Resources.Load<GameObject>("Prefabs/ScreenEffect");
        GameObject inst = Instantiate(prefab);
        fadeImage = inst.transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// ���̵� �ƿ��� �Բ� ���� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="name">��ȯ�� ���� �̸��Դϴ�.</param>
    public void ChangeScene(string name)
    {
        if (isFading) return;
        isFading = true;
        StartCoroutine(CFadeOut( () => {
            SceneManager.LoadScene(name);
            StartCoroutine(CFadeIn(null));
        }));
    }

    /// <summary>
    /// ������ �Ͻ����� ���θ� �����մϴ�.
    /// </summary>
    /// <param name="active">true�� �� �Ͻ�����</param>
    public void SetPause(bool active)
    {
        Time.timeScale = active ? 0.0f : 1.0f;
    }

    private IEnumerator CFadeOut(Callback c)
    {
        while (fadeImage.color.a < 1.0f)
        {
            Color newColor = fadeImage.color;
            newColor.a += Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        SetPause(true);
        yield return new WaitForSecondsRealtime(transitionDelay);
        c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        while (fadeImage.color.a > 0)
        {
            Color newColor = fadeImage.color;
            newColor.a -= Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        c();
        SetPause(false);
        isFading = false;
    }
}
