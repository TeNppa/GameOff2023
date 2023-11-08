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
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        mapManager = GroundTileMap.GetComponent<MapManager>();
        playerCtrlr = Player.GetComponent<PlayerController>();
        ConnectEvents();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConnectEvents()
    {
        playerCtrlr.OnDig += mapManager.Dig;
    }
}
