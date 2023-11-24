using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightLevel : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Light2D globalLight;

    [SerializeField] private float intensityAboveGround = 1.0f;
    [SerializeField] private float intensityBelowGround = 0.05f;
    [SerializeField] private float transitionSpeed = 1.0f;
    [SerializeField] private float transitionPoint = -1.0f;
    [SerializeField] private float intensityThreshold = 0.01f;


    private void Start()
    {
        globalLight = GetComponent<Light2D>();
    }


    void Update()
    {
        UpdateLightIntensity();
    }


    private void UpdateLightIntensity(bool forceUpdate = false)
    {
        float targetIntensity = player.position.y > transitionPoint ? intensityAboveGround : intensityBelowGround;

        if (Mathf.Abs(globalLight.intensity - targetIntensity) > intensityThreshold || forceUpdate)
        {
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetIntensity, transitionSpeed * Time.deltaTime);
        }
    }
}
