using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float scrollSpeed = 0.025f;
    [SerializeField] private float yWaveAmplitude = 0.05f;
    [SerializeField] private float yWaveFrequency = 1f;
    private float yWaveTime = 0f;

    void Update()
    {
        yWaveTime += Time.deltaTime;
        float yWaveOffset = Mathf.Sin(yWaveTime * yWaveFrequency) * yWaveAmplitude;
        meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, yWaveOffset * Time.deltaTime);
    }
}