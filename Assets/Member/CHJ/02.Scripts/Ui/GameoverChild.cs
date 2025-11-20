using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Member.CHJ._02.Scripts.Ui
{
    public class GameoverChild : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _child;
        public void Show()
        {
            foreach (var child in _child)
            {
                child.DOFade(1, 1.2f);
            }
        }
    }
}