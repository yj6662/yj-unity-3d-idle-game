using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    
    [Header("맵 생성 설정")] 
    public GameObject[] rockPrefabs;
    public int numberOfRocks;
    public float generationMinX = -60f;
    public float generationMaxX = -10f;
    public float generationLength = 300f;
    
    public Transform rockParent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateMap()
    {
        // 지정된 범위 내에 무작위 위치에 무작위 크기 범위의 바위 생성
        ClearMap(); // 이전 스테이지의 바위 삭제

        for (int i = 0; i < numberOfRocks; i++)
        {
            float randomX = Random.Range(generationMinX, generationMaxX);
            float randomZ = Random.Range(0, generationLength);
            Vector3 spawnPosition = new Vector3(randomX, 0, randomZ);
            
            GameObject randomRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            float randomScale = Random.Range(3f, 10f);
            
            GameObject rockInstance = Instantiate(randomRockPrefab, spawnPosition, spawnRotation);
            rockInstance.transform.localScale = Vector3.one * randomScale;
            rockInstance.transform.SetParent(rockParent);
        }
    }

    private void ClearMap()
    {
        if (rockParent == null) return;

        foreach (Transform rock in rockParent)
        {
            Destroy(rock.gameObject);
        }
    }
}
