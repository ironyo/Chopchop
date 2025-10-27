using DG.Tweening;
using UnityEngine;

public class UI_UpDown : MonoBehaviour
{
    [Tooltip("처음위치")]
    [SerializeField] private Vector2 Pos_01;
    [Tooltip("나중위치")]
    [SerializeField] private Vector2 Pos_02;

    [Header("움직일 오브젝트")]
    [SerializeField] private RectTransform MoveObejct;

    [Header("움직이는 시간")]
    [SerializeField] private float MoveTime;

    private bool isPos01 = true;
    public void BtnClick()
    {
        if (isPos01)
        {
            MoveObejct.DOAnchorPos(Pos_02, MoveTime);
            isPos01 = false;
        }
        else
        {
            MoveObejct.DOAnchorPos(Pos_01, MoveTime);
            isPos01 = true;
        }

    }
}
