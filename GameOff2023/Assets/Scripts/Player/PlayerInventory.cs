using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int GoldAmount;
    [SerializeField]
    private int DirtAmount;

    public void AddDirt(int amount)
    {
        DirtAmount += amount;
    }
    public void AddGold(int amount)
    {
        GoldAmount += amount;
    }

    public bool UseGold(int amount)
    {
        if (amount > GoldAmount)
            return false;
        GoldAmount -= amount;
        return true;
    }
}
