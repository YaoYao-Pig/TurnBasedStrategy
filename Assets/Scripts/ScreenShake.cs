using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;
    public static ScreenShake Instance { get; set; }
    private void Awake()
    {
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
