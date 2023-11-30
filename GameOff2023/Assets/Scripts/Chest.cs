using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Text lootText;
    [SerializeField] private Text rewardText;
    [SerializeField] private int minGoldReward = 5;
    [SerializeField] private int maxGoldReward = 25;
    [SerializeField] private LayerMask groundLayer;
    private Animator animator;
    private PlayerInventory playerInventory;
    private bool isOpened = false;


    void Start()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        playerInventory = gameManager.GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        lootText.text = "";
        Invoke("PositionChestOnGround", 2);
    }


    private void PositionChestOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, groundLayer);
        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.6f, transform.position.z);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.gameObject.tag == "Player" )
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenChest();
            }

            lootText.text = "E";
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (isOpened) return;

        if (other.gameObject.tag == "Player")
        {
            lootText.text = "";
        }
    }


    private void OpenChest()
    {
        isOpened = true;
        animator.SetTrigger("Open Chest");
        int goldReward = Random.Range(minGoldReward, maxGoldReward + 1);
        playerInventory.AddValuable(0, goldReward);
        rewardText.text = goldReward.ToString();
    }
}
