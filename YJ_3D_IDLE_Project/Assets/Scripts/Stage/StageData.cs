using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageData : ScriptableObject
{
    [Header("스테이지 정보")]
    public string stageName;
    public string stageDescription;
    public GameObject[] enemyPrefabs;

    [Header("적 소환 위치")] public Vector3[] spawnPoints;
}
