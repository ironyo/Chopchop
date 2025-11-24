using System;
using UnityEngine;

public class SaveComponent : MonoBehaviour
{
    public WeaponHolder weapon;

    private void Awake()
    {
        weapon = GetComponentInChildren<WeaponHolder>();
    }
}
