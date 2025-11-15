using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TestMinionManager : MonoSingleton<TestMinionManager>
{
    public List<TestMinion> alivesMinions = new List<TestMinion>();
}
