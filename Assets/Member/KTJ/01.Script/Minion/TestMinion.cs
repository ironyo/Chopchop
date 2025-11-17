using System;
using System.Collections;
using UnityEngine;

public class TestMinion : MonoBehaviour
{
    [SerializeField] private MinionChat minionChat;

    [Header("Particels")]
    [SerializeField] private ParticleSystem AppleParticels;
    [SerializeField] private ParticleSystem WaterParticels;
    [SerializeField] private ParticleSystem CleanParticels;
    [SerializeField] private ParticleSystem BombParticles;
    [SerializeField] private ParticleSystem MopeParticles;

    [Header("Audio clip")]
    [SerializeField] private AudioClip EatSound;
    [SerializeField] private AudioClip DrinkSound;
    [SerializeField] private AudioClip BrushSound;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public int Mood { get; private set; } // 0 ~ 100 (배고픔, 목마름, 더러움 세 수치의 평균)
    public int Hungry { get; private set; } // 0~100
    public int Thirsty { get; private set; } // 0~100
    public int Dirty { get; private set; } // 0~100

    IEnumerator WaitDestroy(int wait)
    {
        yield return new WaitForSeconds(wait);  
        Debug.Log("미니언 삭제");
        Destroy(gameObject);
    }
    public void Die()
    {
        minionChat.AddMessage("죽음");
        TestMinionManager.Instance.alivesMinions.Remove(this);
        StartCoroutine(WaitDestroy(1));
    }

    public void Bomb()
    {
        minionChat.AddMessage("너무 숨막혀.. 터진다!");
        animator.SetTrigger("Bomb");
    }

    public void OnBombAnimEnd()
    {
        BombParticles.Play();
    }

    public void Mope()
    {
        MopeParticles.Play();
        minionChat.AddMessage("친구들이 없어..");
        minionChat.AddMessage("우울하다 ㅠㅠ");
    }
    public void UnMope()
    {
        MopeParticles.Stop();
        minionChat.AddMessage("이제 우울하지 않아!");
    }

    public void EatApple(int amount)
    {
        Hungry = Mathf.Clamp(amount + Hungry, 0, 100);
        Debug.Log("현재 미니언 배고픔: " + Hungry);
        minionChat.AddMessage("우걱우걱");
        AppleParticels.Play();

        SoundManager.Instance.SFXPlay("EatSound",EatSound);
    }

    public void EatWater(int amount)
    {
        Thirsty = Mathf.Clamp(amount + Thirsty, 0, 100);
        Debug.Log("현재 미니언 목마름: " + Thirsty);
        minionChat.AddMessage("꿀꺽꿀꺽!");
        WaterParticels.Play();

        SoundManager.Instance.SFXPlay("DrinkSound", DrinkSound);
    }

    public void Clean(int amount)
    {
        Dirty = Mathf.Clamp(amount + Dirty, 0, 100);
        Debug.Log("현재 미니언 더러움: " + Dirty);
        minionChat.AddMessage("깨끗해졌다");
        CleanParticels.Play();

        SoundManager.Instance.SFXPlay("BrushSound", BrushSound);
    }
}
