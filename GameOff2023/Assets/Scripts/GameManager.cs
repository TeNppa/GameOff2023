using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private GameObject GroundTileMap;
    [SerializeField] 
    private GameObject Player;

    private PlayerController playerCtrlr;
    private MapManager mapManager;

    public PlayerInventory playerInventory;
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        mapManager = GroundTileMap.GetComponent<MapManager>();
        playerCtrlr = Player.GetComponent<PlayerController>();
        playerInventory = gameObject.GetComponent<PlayerInventory>();
        ConnectEvents();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGround(int ground, int amount)
    {
        Debug.Log("In gm");
            playerInventory.AddGround(ground, amount);
    }

    public void AddValuable(int valuable, int amount)
    {
        Debug.Log("In gm");
        playerInventory.AddValuable(valuable, amount);
    }

    void ConnectEvents()
    {
        playerCtrlr.OnDig += mapManager.Dig;
        mapManager.OnEndDig += playerCtrlr.EndDig;
    }
}
