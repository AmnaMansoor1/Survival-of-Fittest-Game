using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MorningTransition : MonoBehaviour
{
    [Header("UI Text")]
    public GameObject dawnText;

    [Header("Light Settings")]
    public Light directionalLight;

    public Color nightLightColor = new Color(0.25f, 0.35f, 0.6f);
    public Color morningLightColor = new Color(1f, 0.75f, 0.45f);

    public float nightIntensity = 0.4f;
    public float morningIntensity = 1.2f;

    [Header("Sky Settings")]
    public Camera mainCamera;

    public Color nightSkyColor = new Color(0.02f, 0.03f, 0.08f);
    public Color morningSkyColor = new Color(0.35f, 0.65f, 1f);

    [Header("Ambient Light Settings")]
    public Color nightAmbientColor = new Color(0.08f, 0.08f, 0.12f);
    public Color morningAmbientColor = new Color(0.55f, 0.60f, 0.70f);

    [Header("Sun Rotation")]
    public Vector3 nightSunRotation = new Vector3(20f, -30f, 0f);
    public Vector3 morningSunRotation = new Vector3(55f, -30f, 0f);

    [Header("Transition Settings")]
    public float transitionDuration = 4f;
    public string winSceneName = "WinScene";

    private bool transitionStarted = false;

    public void StartMorningTransition()
    {
        if (transitionStarted) return;
        transitionStarted = true;

        if (dawnText != null)
        {
            dawnText.SetActive(true);
        }

        if (mainCamera == null && Camera.main != null)
        {
            mainCamera = Camera.main;
        }

        StartCoroutine(MorningRoutine());
    }

    IEnumerator MorningRoutine()
    {
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;

            if (directionalLight != null)
            {
                directionalLight.color = Color.Lerp(nightLightColor, morningLightColor, t);
                directionalLight.intensity = Mathf.Lerp(nightIntensity, morningIntensity, t);

                directionalLight.transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(nightSunRotation),
                    Quaternion.Euler(morningSunRotation),
                    t
                );
            }

            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.SolidColor;
                mainCamera.backgroundColor = Color.Lerp(nightSkyColor, morningSkyColor, t);
            }

            RenderSettings.ambientLight = Color.Lerp(
                nightAmbientColor,
                morningAmbientColor,
                t
            );

            yield return null;
        }

        SceneManager.LoadScene(winSceneName);
    }
}