using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin CameraNoise;

    private void Awake()
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

    void Start()
    {
        if (virtualCamera != null)
        {
            CameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (CameraNoise != null)
            {
                CameraNoise.m_AmplitudeGain = 0.1f;
            }
        }
    }

    public void ShakeCamera(float intensity, float duration)
    {
        // TakeDamage에서 호출
        if (CameraNoise != null)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }
    }
    
    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        // intensity만큼의 흔들림을 duration초 동안 지속
        CameraNoise.m_AmplitudeGain = intensity;
        
        yield return new WaitForSeconds(duration);
        
        CameraNoise.m_AmplitudeGain = 0.0f;
    }
}
