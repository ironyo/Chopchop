using System;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private float roationSpeed = 2f;
    private float angle;

    public void AimWeapon(Vector3 targetPos)
    {
        if (targetPos == null) return;
        
        Vector3 dir = targetPos - transform.position;
        dir.z = 0;
        
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * roationSpeed);
        AdjustWeaponRendering();
    }

    private void AdjustWeaponRendering()
    {
        FlipSprite(angle > 90 || angle < -90);
    }
    
    private void FlipSprite(bool val)
    {
        int flipX = val ? -1 : 1;
        transform.localScale = new Vector3(transform.localScale.x, flipX * Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }
}
