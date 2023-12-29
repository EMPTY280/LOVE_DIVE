using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Image fadeImage = null;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float transitionDelay = 1.0f;

    private delegate void Callback();
    [SerializeField] private bool isFading = false;
    private string targetScene = "";

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
        GameObject prefab = Resources.Load<GameObject>("Prefabs/ScreenEffect");
        GameObject inst = Instantiate(prefab);
        fadeImage = inst.transform.GetChild(0).GetComponent<Image>();

        Color newColor = fadeImage.color;
        newColor.a = 0f;
        fadeImage.color = newColor;
        fadeImage.gameObject.SetActive(true);
        fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        DontDestroyOnLoad(inst);
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���̵� �ƿ��� �Բ� ���� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="name">��ȯ�� ���� �̸��Դϴ�.</param>
    public void ChangeScene(string name)
    {
        if (isFading) return;
        isFading = true;

        targetScene = name;
        StartCoroutine(CFadeOut( () =>
        {
            SceneManager.LoadScene(targetScene);
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
        Debug.Log("in");
        while (fadeImage.color.a < 1.0f)
        {
            Color newColor = fadeImage.color;
            newColor.a += Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(transitionDelay);
        if (c != null) c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        Debug.Log("out");
        while (fadeImage.color.a > 0)
        {
            Color newColor = fadeImage.color;
            newColor.a -= Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        if (c != null) c();
        isFading = false;
    }
}
