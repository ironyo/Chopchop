using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneChangeManager : MonoSingleton<SceneChangeManager>
{
    [SerializeField] private GameObject ChangePref;
    [SerializeField] private string[] RandomTip;
    private RectTransform Canvas;
    private Sequence seq;
    private bool isSceneMoving = false;

    private string currentTip;

    public UnityEvent OnSceneChangeLoaded;

    protected override void Awake()
    {
        base.Awake();
        seq = DOTween.Sequence();

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentTip = RandomTip[Random.Range(0, RandomTip.Length)];
    }

    private void OnDestroy()
    {
        // �޸� ���� ������ ���� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas = GameObject.Find("Canvas").gameObject.GetComponent<RectTransform>();

        if (scene.buildIndex != 0)
        {
            OnSceneStart();
        }
    }

    public void ExitStartScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (isSceneMoving == false)
            {
                    OnSceneEnd(1);
            }
        }
    }
    public void OnSceneStart()
    {
        isSceneMoving = true;

        seq?.Kill();
        seq = DOTween.Sequence();

        SceneChangePref changePref = Instantiate(ChangePref, Canvas).GetComponent<SceneChangePref>();

        changePref.TipTxt.text = currentTip;

        seq.Append(changePref.TextGroup.DOFade(0, 1f));
        seq.Append(changePref.MoveObject.DOAnchorPosY(changePref.HidePosY, 1.5f));
        seq.OnComplete(() =>
        {
            isSceneMoving = false;
            OnSceneChangeLoaded.Invoke();
        });
    }

    public void OnSceneEnd(int sceneIdx)
    {
        isSceneMoving = true;

        currentTip = RandomTip[Random.Range(0, RandomTip.Length)];

        seq?.Kill();
        seq = DOTween.Sequence();

        SceneChangePref changePref = Instantiate(ChangePref, Canvas).GetComponent<SceneChangePref>();

        changePref.TipTxt.text = currentTip;

        changePref.MoveObject.anchoredPosition = new Vector2(0, changePref.HidePosY);
        changePref.TextGroup.alpha = 0;

        seq.Append(changePref.MoveObject.DOAnchorPosY(0, 1.5f));
        seq.Append(changePref.TextGroup.DOFade(1f, 1f));
        seq.AppendInterval(1f);

        seq.OnComplete(() =>
        {
            SceneManager.LoadScene(sceneIdx);
            isSceneMoving = false;
        });
    }
}
