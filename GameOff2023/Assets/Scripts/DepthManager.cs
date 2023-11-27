using UnityEngine;
using UnityEngine.UI;

public class DepthManager : MonoBehaviour
{
    [SerializeField] private Text depthText;
    [SerializeField] private Transform player;
    private float personalBest = 0f;
    private float currentDepth = 0f;

    void Update()
    {
        currentDepth = Mathf.Floor(-player.transform.position.y);

        if (currentDepth > personalBest)
        {
            personalBest = currentDepth;
        }

        depthText.text = currentDepth + " meters (best: " + personalBest + " meters)";
    }
}