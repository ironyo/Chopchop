using Member.CHJ._02.Scripts;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMinion : MonoBehaviour
{
    [SerializeField] private MinionChat minionChat;
    [SerializeField] private GameObject weapon;

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

    [SerializeField] Animator animator;
    
    public WeaponHolder weaponHolder;

    private void Awake()
    {
        weaponHolder = GetComponentInChildren<WeaponHolder>();
    }


    public int Mood { get; private set; } // 0 ~ 100 (배고픔, 목마름, 더러움 세 수치의 평균)

    private int _hungry;
    public int Hungry
    {
        get => _hungry;
        set => _hungry = Mathf.Clamp(value, 0, 100);
    }

    private int _thirsty;
    public int Thirsty
    {
        get => _thirsty;
        set => _thirsty = Mathf.Clamp(value, 0, 100);
    }

    private int _dirty;
    public int Dirty
    {
        get => _dirty;
        set => _dirty = Mathf.Clamp(value, 0, 100);
    }

    private void Start() // 만땅으로 채우기
    {
        Hungry = 50;
        Thirsty = 50;
        Dirty = 50;
    }
    IEnumerator WaitDestroy(int wait)
    {
        yield return new WaitForSeconds(wait);  
        Destroy(gameObject);
    }
    public void Die(string message = null)
    {
        if (message == null || message == "")
        {
            minionChat.AddMessage("죽음");
        }
        else
        {
            minionChat.AddMessage(message);
        }
        MinionManager.Instance.minionList.Remove(gameObject.GetComponent<Minion>());
        StartCoroutine(WaitDestroy(1));
    }
    public void Bomb()
    {
        minionChat.AddMessage("너무 숨막혀.. 터진다!");
        animator.SetTrigger("Bomb");
        weapon.SetActive(false);
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
        minionChat.AddMessage("이동속도가 {2}로 감소함");
    }
    public void UnMope()
    {
        if (!this || this.gameObject == null) return;
        if (MopeParticles == null) return;

        MopeParticles.Stop();

        minionChat.AddMessage("이제 우울하지 않아!");
        minionChat.AddMessage("이동속도가 {5}로 정상화");
    }


    public void EatApple(int amount)
    {
        Hungry += amount;

        minionChat.AddMessage("우걱우걱");
        AppleParticels.Play();

        SoundManager.Instance.SFXPlay("EatSound",EatSound);

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (TutorialManager.Instance.GetCurrentStepId() == "prey")
            {
                TutorialManager.Instance.CompleteCurrentStepExternally();
            }
        }
    }

    public void EatWater(int amount)
    {
        Thirsty += amount;

        minionChat.AddMessage("꿀꺽꿀꺽!");
        WaterParticels.Play();

        SoundManager.Instance.SFXPlay("DrinkSound", DrinkSound);
    }

    public void Clean(int amount)
    {
        Dirty += amount;

        minionChat.AddMessage("깨끗해졌다");
        CleanParticels.Play();

        SoundManager.Instance.SFXPlay("BrushSound", BrushSound);
    }
}
