using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BombEvent : MonoBehaviour, IGameEvent
{
    int bombDuration = 3;
    float currentTime = 0;

    bool isRunning = false;

    private void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime > bombDuration)
            {
                currentTime = 0;
                BoomRandMinion();
            }
        }
    }

    private void BoomRandMinion()
    {
        List<TestMinion> minions = TestMinionManager.Instance.alivesMinions;

        if (minions.Count > 0 )
        {
            TestMinion pickedMinion = minions[Random.Range(0, minions.Count)];

            pickedMinion.Bomb();
        }
    }

    #region run&stop
    public void Run()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
        Debug.Log("BombEvent ÁßÁö");
    }
    #endregion
}
