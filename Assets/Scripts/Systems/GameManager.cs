using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private Image fadeImage = null;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float transitionDelay = 1.0f;

    /// <summary>
    /// ���̵� ��/�ƿ��� �Ϸ�Ǳ������ �ð��Դϴ�. (N��, ��/�ƿ��� ���� �����, �ּ� 0.1��)
    /// </summary>
    public float FadeTime
    {
        get { return fadeTime; }
        set { fadeTime = Mathf.Min(0.1f, value); }
    }

    /// <summary>
    /// ���̵� �ƿ� �� ���̵� ���� ���۵Ǳ������ ��� �ð��Դϴ�. (N��, �ּ� 0��)
    /// </summary>
    public float TransitionDelay
    {
        get { return transitionDelay; }
        set { transitionDelay = Mathf.Min(0f, value); }
    }

    private delegate void Callback();
    [SerializeField] private bool isFading = false;
    private string targetScene = "";

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<GameManager>();
                instance.name = "[ GameManager ]";
            }
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
        while (fadeImage.color.a < 1.0f)
        {
            Color newColor = fadeImage.color;
            newColor.a += Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        SetPause(true);
        yield return new WaitForSecondsRealtime(transitionDelay);
        if (c != null) c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        SetPause(false);
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
