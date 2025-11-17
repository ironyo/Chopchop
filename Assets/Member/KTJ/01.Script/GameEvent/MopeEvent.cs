using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Member.CHJ._02.Scripts;
using Unity.VisualScripting;

public class MopeEvent : MonoBehaviour, IGameEvent
{
    private bool isRunning = false;

    int pickDuration = 3;
    float currentTime = 0;

    List<TestMinion> mopedMinions = new List<TestMinion>();
    private void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime > pickDuration)
            {
                currentTime = 0;
                var pickedMinion = PickMopeMinion();
                if (pickedMinion != null)
                {
                    pickedMinion.Mope();
                }
            }
        }
    }
    private TestMinion PickMopeMinion()
    {
        var minions = MinionManager.Instance.minionList;

        var availableMinions = minions.Where(m => !mopedMinions.Contains(m.GetComponent<TestMinion>())).ToList();

        if (availableMinions.Count > 0)
        {
            TestMinion pickedMinion = availableMinions[Random.Range(0, availableMinions.Count)].GetComponent<TestMinion>();
            mopedMinions.Add(pickedMinion);
            return pickedMinion;
        }

        return null;
    }
    private void UnMopeAll()
    {
        mopedMinions.ForEach(x => x.UnMope());
        mopedMinions.Clear();
    }
    public void Run()
    {
        isRunning = true;
    }

    public void Stop()
    {
        UnMopeAll();
        isRunning = false;
    }
}
