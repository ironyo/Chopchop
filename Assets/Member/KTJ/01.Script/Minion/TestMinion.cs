using System;
using UnityEngine;

public class TestMinion : MonoBehaviour
{
    [SerializeField] private MinionChat minionChat;

    [Header("Particels")]
    [SerializeField] private ParticleSystem AppleParticels;
    [SerializeField] private ParticleSystem WaterParticels;
    [SerializeField] private ParticleSystem CleanParticels;


    public int Hungry; // 0~100
    public int Thirsty; // 0~100
    public int Dirty; // 0~100

    public void EatApple(int amount)
    {
        Hungry = Mathf.Clamp(amount + Hungry, 0, 100);
        Debug.Log("���� �̴Ͼ� �����: " + Hungry);
        minionChat.AddMessage("��ƿ��");
        AppleParticels.Play();
    }

    public void EatWater(int amount)
    {
        Thirsty = Mathf.Clamp(amount + Thirsty, 0, 100);
        Debug.Log("���� �̴Ͼ� �񸶸�: " + Thirsty);
        minionChat.AddMessage("�ܲ��ܲ�!");
        WaterParticels.Play();
    }

    public void Clean(int amount)
    {
        Dirty = Mathf.Clamp(amount + Dirty, 0, 100);
        Debug.Log("���� �̴Ͼ� ������: " + Dirty);
        minionChat.AddMessage("����������");
        CleanParticels.Play();
    }
}
