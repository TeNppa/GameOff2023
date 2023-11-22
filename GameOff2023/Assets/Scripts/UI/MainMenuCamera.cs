using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Transform leaderboardPosition;
    [SerializeField] private Transform creditsPosition;
    [SerializeField] private float speed = 1.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float journeyLength;
    private float startTime;


    private void Start()
    {
        MoveToDefaultPosition();
    }


    private void Update()
    {
        AnimateCamera();
    }


    private void AnimateCamera()
    {
        animator.SetBool("showMenu", transform.position.y == defaultPosition.position.y);
        animator.SetBool("showCredits", transform.position.y == creditsPosition.position.y);

        // Move camera towards target position
        float newY = Mathf.SmoothStep(startPosition.y, endPosition.y, (Time.time - startTime) * speed / journeyLength);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }


    public void MoveToDefaultPosition()
    {
        SetDestination(defaultPosition.position.y);
    }


    public void MoveToLeaderboardPosition()
    {
        SetDestination(leaderboardPosition.position.y);
    }


    public void MoveToCreditsPosition()
    {
        SetDestination(creditsPosition.position.y);
    }


    private void SetDestination(float posY)
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, posY, transform.position.z);
        startTime = Time.time;
        journeyLength = Mathf.Abs(startPosition.y - posY);
    }
}