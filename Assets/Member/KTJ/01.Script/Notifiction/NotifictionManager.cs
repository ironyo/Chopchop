using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NotifictionManager : MonoBehaviour
{
    public static NotifictionManager Instance;

    [SerializeField] private RectTransform NotificAnc; // 알림창 움직이는거
    [SerializeField] private TextMeshProUGUI TitleTxt;
    [SerializeField] private TextMeshProUGUI TitleShadowTxt;

    [SerializeField] private TextMeshProUGUI DescTxt;
    [SerializeField] private RectTransform AlarmIcon;

    private float startPosX;
    private Sequence seq;

    public UnityEvent<string, string> NotifictionEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        seq = DOTween.Sequence();
    }

    private void Start()
    {
        NotifictionEvent.AddListener(NotificRun);
        startPosX = NotificAnc.anchoredPosition.x;
    }

    private void NotificRun(string title, string desc)
    {
        seq?.Kill();
        seq = DOTween.Sequence();

        NotificAnc.anchoredPosition = new Vector2(startPosX, 0);
        TitleTxt.text = title;
        TitleShadowTxt.text = title;
        DescTxt.text = desc;

        seq.Append(NotificAnc.DOAnchorPosX(0f, 1f));

        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, -20), 0.2f));
        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, 20), 0.1f));
        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, -20), 0.1f));
        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, 20), 0.1f));
        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, -20), 0.1f));
        seq.Append(AlarmIcon.DORotate(new Vector3(0, 0, 0), 0.2f));

        seq.AppendInterval(2.5f);
        seq.Append(NotificAnc.DOAnchorPosX(startPosX, 1f));
    }
}
