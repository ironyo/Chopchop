using UnityEngine;
using UnityEngine.UI;

public class NowBuildingTimer : MonoBehaviour
{
    Building _buildCompo;
    SpriteRenderer _sr;
    MaterialPropertyBlock _mpb;
    Image _timerImg;

    float _buildingTime;
    float _currentTime;

    public float _realValue = -1.5f;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
    }
    private void Start()
    {
        _sr.sprite = _buildCompo.buildingSO.buildSprite;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
        float t = _currentTime / _buildingTime;
        _realValue = Mathf.Lerp(-1.5f, 1.5f, t);
        _timerImg.fillAmount = Mathf.Lerp(0, 1, t);
        SetValue(_realValue);
        if (_currentTime >= _buildingTime)
        {
            _buildCompo.BuildingRealClear();
            Destroy(_timerImg.gameObject);
            Destroy(gameObject);
        }
    }
    public void GetData(Building data, Image timer)
    {
        _buildCompo = data;
        _timerImg = timer;
        _buildingTime = _buildCompo.buildingSO.buildTime;
    }

    private void SetValue(float v)
    {
        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_Value", v);
        _sr.SetPropertyBlock(_mpb);
    }
}
