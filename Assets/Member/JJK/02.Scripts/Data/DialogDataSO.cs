using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDataSO", menuName = "SO/DialogDataSO")]
public class DialogDataSO : ScriptableObject
{
    public string name;
    
    [TextArea]
    public string[] explain;
}
