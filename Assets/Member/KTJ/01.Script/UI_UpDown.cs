using DG.Tweening;
using UnityEngine;

public class UI_UpDown : MonoBehaviour
{
    [Tooltip("ó����ġ")]
    [SerializeField] private Vector2 Pos_01;
    [Tooltip("������ġ")]
    [SerializeField] private Vector2 Pos_02;

    [Header("������ ������Ʈ")]
    [SerializeField] private RectTransform MoveObejct;

    [Header("�����̴� �ð�")]
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
