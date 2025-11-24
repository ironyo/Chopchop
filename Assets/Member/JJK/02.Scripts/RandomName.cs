using UnityEngine;

public class RandomName
{
    static readonly string[] first =
    {
        "에스", "칼", "라그", "벨", "마르", "세렌", "타라", "오르", "카르",
        "리브", "아크", "델피", "솔", "노바", "아란", "파르"
    };

    static readonly string[] second =
    {
        "라", "나", "레", "리", "아", "에", "온", "타", "로", "비", "사",
        "엘", "라스", "린", "몬", "투", "바"
    };
    
    public static string CreateIslandName()
    {
        string p = first[Random.Range(0, first.Length)];
        string m = second[Random.Range(0, second.Length)];

        return p + m;
    }
}
