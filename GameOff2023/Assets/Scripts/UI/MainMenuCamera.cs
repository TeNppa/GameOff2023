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
    private bool isInitialMovementDone = false;


    private void Start()
    {
        MoveToDefaultPosition();
    }


    private void Update()
    {
        if (journeyLength != 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            if (!isInitialMovementDone && fractionOfJourney >= 1.0f)
            {
                isInitialMovementDone = true;
            }

            float newY;
            if (isInitialMovementDone)
            {
                newY = Mathf.SmoothStep(startPosition.y, endPosition.y, fractionOfJourney);
            }
            else
            {
                newY = Mathf.Lerp(startPosition.y, endPosition.y, fractionOfJourney);
            }


            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        animator.SetBool("showMenu", transform.position.y == defaultPosition.position.y);
        animator.SetBool("showCredits", transform.position.y == creditsPosition.position.y);
    }


    public void MoveToDefaultPosition()
    {
        SetYDestination(defaultPosition.position.y);
    }


    public void MoveToLeaderboardPosition()
    {
        SetYDestination(leaderboardPosition.position.y);
    }


    public void MoveToCreditsPosition()
    {
        SetYDestination(creditsPosition.position.y);
    }


    private void SetYDestination(float newY)
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, newY, transform.position.z);
        startTime = Time.time;
        journeyLength = Mathf.Abs(startPosition.y - newY);
    }
}