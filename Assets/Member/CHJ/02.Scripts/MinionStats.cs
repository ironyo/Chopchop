using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class MinionStats
{
    public int Hp { get; set; }
    public float Hunger { get; set; }
    public float Emotion { get; set; }
    public int Age { get; set; }
    public string Name { get; private set; }

    public WorkActionScr Job { get; set; }


    private List<String> NameList = new List<string>()
    {
        "강유","김태준","정재근","최정우","염승민","김용민","남기태","김예루",
        "백범직","박태윤","유현우","노현우","전창준","박철민","지태환","조윤규"
    };

    public MinionStats()
    {
        Hp = Random.Range(10, 30);
        Hunger = 100;
        Emotion = 100;
        Age = 1;
        Name = NameList[Random.Range(0, NameList.Count)];
    }
}
