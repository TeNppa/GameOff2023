using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ValuablesInventory> valuables;
    [SerializeField] private List<GroundsInventory> grounds;
    [SerializeField] private int torches;
    [SerializeField] private int staminaPotions;


    public void AddGround(int ground, int amount)
    {
        // Add ground to inventory
        grounds.FirstOrDefault(g => (int)g.Ground == ground)!.Amount += amount;
    }

    public void UseGround(int ground, int amount)
    {
        // Skipped, as there is not enough time to implement building mechanics
    }

    public void AddValuable(int valuable, int amount)
    {
        // Add valuables to inventory
        valuables.FirstOrDefault(v => (int)v.Valuable == valuable)!.Amount += amount;
    }

    public void UseValuable(Valuables valuable, int amount)
    {
        // Complete a purchase and spend the valuables
        var inventoryItem = valuables.FirstOrDefault(v => v.Valuable == valuable);
        if (inventoryItem != null && inventoryItem.Amount >= amount)
        {
            inventoryItem.Amount -= amount;
        }
    }

    public bool CheckValuableAmount(Valuables valuable, int requiredAmount)
    {
        // Make sure player has required valuables before the purchase is done
        ValuablesInventory inventoryItem = valuables.FirstOrDefault(v => v.Valuable == valuable);
        return inventoryItem != null && inventoryItem.Amount >= requiredAmount;
    }

    public void AddTorch(int amount)
    {
        torches += amount;
    }

    public void RemoveTorch(int amount)
    {
        torches -= amount;
    }

    public bool HasTorches()
    {
        return torches > 0;
    }

    public void AddStaminaPotion(int amount)
    {
        staminaPotions += amount;
    }

    public void RemoveStaminaPotion(int amount)
    {
        staminaPotions -= amount;
    }

    public bool HasStaminaPotions()
    {
        return staminaPotions > 0;
    }
}


[Serializable]
public class ValuablesInventory
{
    public Valuables Valuable;
    public int Amount;
}


[Serializable]
public class GroundsInventory
{
    public Grounds Ground;
    public int Amount;
}