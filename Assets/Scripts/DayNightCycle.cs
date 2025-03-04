using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float time; // �ð�
    public float fullDayLength; // �Ϸ� ����
    public float startTime = 0.4f; // ���� ������ ��, �ð�

    private float timeRate; // �ð� �帣�� �ֱ�
    public Vector3 noon; // Vector 90 0 0 (����)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // �׶��̼��� �༭ ������ ���� ���ϰ�
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor; // �׶��̼��� �༭ ������ ���� ���ϰ�
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // �ִϸ��̼� Ŀ�긦 ����� ���� ���� ����
    public AnimationCurve reflectionIntencityMultiplier; // �ݻ� ���� ����

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // �帣�� �ð� �ӵ� ����
        time = startTime; // ���� �ð� �ʱ� ����
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // �ð� �帣�� �ϱ�
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
