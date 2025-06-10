using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageList", menuName = "ScriptableObjects/StageList")]
public class StageList : ScriptableObject
{
    public List<StageData> stages;
}
