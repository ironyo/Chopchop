using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Member.CHJ._02.Scripts;

public class BombEvent : MonoBehaviour, IGameEvent
{
    float bombDuration = 0.8f;
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
        if (MinionManager.Instance.minionList == null)
            return;
        TestMinion minion = MinionManager.Instance.minionList
            [Random.Range(0, MinionManager.Instance.minionList.Count)].GetComponent<TestMinion>();
        minion.Bomb();
    }

    #region run&stop
    public void Run()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }
    #endregion
}
