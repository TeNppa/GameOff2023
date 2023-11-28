using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Text lootText;
    [SerializeField] private Text rewardText;
    [SerializeField] private int minGoldReward = 5;
    [SerializeField] private int maxGoldReward = 25;
    private Animator animator;
    private PlayerInventory playerInventory;
    private bool isOpened = false;


    void Start()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        playerInventory = gameManager.GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        lootText.text = "";
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
        lootText.text = "";
    }
}
