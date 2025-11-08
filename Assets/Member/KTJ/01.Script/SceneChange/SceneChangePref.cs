using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangePref : MonoBehaviour
{
    [field:SerializeField] public CanvasGroup TextGroup { get; private set; }
    [field:SerializeField] public Image Background { get; private set; }
    [field: SerializeField] public RectTransform MoveObject { get; private set; }
    [field:SerializeField] public float HidePosY { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TipTxt { get; private set; }

}
