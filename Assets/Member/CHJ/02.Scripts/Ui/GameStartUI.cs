using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameStartUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected GameObject arrow;
    [SerializeField] protected Transform spawnPos;
    private GameObject _spawnedObj;
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, 0.2f);
        _spawnedObj = Instantiate(arrow, spawnPos);
        _spawnedObj.transform.rotation = Quaternion.Euler(0,0,270);
        _spawnedObj.transform.DOScale(new Vector3(50,50, 0), 0.1f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_spawnedObj == null) return;
        transform.DOScale(1f, 0.2f);
        _spawnedObj.transform.DOScale(new Vector3(0, 0, 0), 0.2f).OnComplete(() =>Destroy(_spawnedObj));
    }
}
