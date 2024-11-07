using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin perlinNoise;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin bossPerlinNoise;
    [SerializeField] private CinemachineFramingTransposer bossFramingTransposer;

    [SerializeField] private float cameraDistance;
    [SerializeField] private float cameraFov;
    [SerializeField] private float duration;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        #region Components
        GameObject playerVirtualCamera = GameObject.FindWithTag("PlayerCamera");
        GameObject bossVirtualCamera = GameObject.FindWithTag("BossCamera");

        playerCamera = playerVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        bossCamera = bossVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        perlinNoise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bossPerlinNoise = bossCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bossFramingTransposer = bossCamera.GetCinemachineComponent <CinemachineFramingTransposer>();
        ResetIntensity();
        #endregion
    }

    #region CameraShake
    public void ShakeCamera(float intensity, float shakeTime)
    {
        perlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(WaitTime(shakeTime));
    }

    IEnumerator WaitTime(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }

    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }

    public void BossShakeCamera(float intensity, float shakeTime)
    {
        bossPerlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(WaitTime(shakeTime));
    }
    #endregion

    public void BossCutscene()
    {
        if (bossCamera != null) bossCamera.Priority = 10;
        if (playerCamera != null) playerCamera.Priority = 0;
        StartCutsceneZoomOut(cameraDistance, cameraFov, duration);
    }

    public void EndBossCutScene()
    {
        bossFramingTransposer.m_CameraDistance = 3f;
        bossCamera.m_Lens.FieldOfView = 40f;

        if (bossCamera != null && bossCamera.Priority >= 10) bossCamera.Priority = 0;
        if (playerCamera != null) playerCamera.Priority = 10;
    }

    #region Zoom Out Cutscene
    public void StartCutsceneZoomOut(float cameraDistance, float cameraFov, float duration)
    {
        StartCoroutine(CutsceneZoomOutCoroutine(cameraDistance, cameraFov, duration));
    }

    private IEnumerator CutsceneZoomOutCoroutine(float cameraDistance, float cameraFov, float duration)
    {
        if (bossFramingTransposer == null) yield break;

        float initialDistance = bossFramingTransposer.m_CameraDistance;
        float initialFOV = bossCamera.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            bossFramingTransposer.m_CameraDistance = Mathf.Lerp(initialDistance, cameraDistance, t);
            bossCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, cameraFov, t);

            yield return null;
        }

        bossFramingTransposer.m_CameraDistance = cameraDistance;
        bossCamera.m_Lens.FieldOfView = cameraFov;
    }
    #endregion
}
