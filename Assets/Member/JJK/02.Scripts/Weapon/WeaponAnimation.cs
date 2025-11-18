using System;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FireWeapon()
    {
        animator.SetTrigger("Fire");
    }
}
