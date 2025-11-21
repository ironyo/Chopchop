using System;
using UnityEngine;
using DG.Tweening;

namespace Member.CHJ._02.Scripts.Ui
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 5;
        private bool _isTrigger = false;
        private void Update()
        {
            Vector3 dir = target.position - transform.position;
            dir.z = 0;
            transform.position += dir.normalized * (speed * Time.deltaTime);
            
            
            Vector2 vp = Camera.main.WorldToViewportPoint(transform.position); // 자기 위치를 뷰포르토 받음
            vp.x = Mathf.Clamp(vp.x, 0.05f, 0.95f); // 카메라 안넘어가게 보정
            vp.y = Mathf.Clamp(vp.y, 0.05f, 0.95f);    
            
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(vp);
            worldPos.z = transform.position.z;
            transform.position = worldPos;
            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.gameObject == target.gameObject)
            {
                Debug.Log("FINDTARGEt");
                _isTrigger = true;
                FindTarget();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.gameObject == target.gameObject)
            {
                _isTrigger = false;
                Debug.Log("Cant Find Target");
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        private void FindTarget()
        {
            transform.position = target.position;
            transform.DOMoveY(4, 0.5f).OnComplete(() => GetComponent<SpriteRenderer>().enabled = false);
        }
    }
}