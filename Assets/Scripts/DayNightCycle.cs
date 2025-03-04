using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float time; // 시간
    public float fullDayLength; // 하루 길이
    public float startTime = 0.4f; // 게임 시작할 때, 시간

    private float timeRate; // 시간 흐르는 주기
    public Vector3 noon; // Vector 90 0 0 (정오)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // 그라데이션을 줘서 서서히 색이 변하게
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor; // 그라데이션을 줘서 서서히 색이 변하게
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // 애니메이션 커브를 사용해 빛의 강도 조절
    public AnimationCurve reflectionIntencityMultiplier; // 반사 강도 조절

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // 흐르는 시간 속도 지정
        time = startTime; // 현재 시간 초기 지정
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간 흐르게 하기
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntencityMultiplier.Evaluate(time);
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0.0f && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0.0f && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
