using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ValuablesInventory> valuables;
    [SerializeField] private List<GroundsInventory> grounds;
    [SerializeField] private int torches;
    [SerializeField] private int staminaPotions;

    [Header("UI Texts")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text amethystText;
    [SerializeField] private Text diamondText;
    [SerializeField] private Text dirtText;
    [SerializeField] private Text stoneText;
    [SerializeField] private Text bedrockText;
    [SerializeField] private Text dragonStoneText;
    [SerializeField] private Text torchesText;
    [SerializeField] private Text staminaPotionsText;


    public void AddGround(int ground, int amount)
    {
        grounds.FirstOrDefault(g => (int)g.Ground == ground)!.Amount += amount;
        UpdateGroundsUI();
    }

    public void UseGround(int ground, int amount)
    {
        // Skipped, as there is not enough time to implement building mechanics
    }

    public void AddValuable(int valuable, int amount)
    {
        valuables.FirstOrDefault(v => (int)v.Valuable == valuable)!.Amount += amount;
        UpdateValuablesUI();
    }

    public void UseValuable(Valuables valuable, int amount)
    {
        // Complete a purchase and spend the valuables
        var inventoryItem = valuables.FirstOrDefault(v => v.Valuable == valuable);
        if (inventoryItem != null && inventoryItem.Amount >= amount)
        {
            inventoryItem.Amount -= amount;
            UpdateValuablesUI();
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
        UpdateTorchesUI();
    }

    public void RemoveTorch(int amount)
    {
        torches -= amount;
        UpdateTorchesUI();
    }

    public bool HasTorches()
    {
        return torches > 0;
    }

    public void AddStaminaPotion(int amount)
    {
        staminaPotions += amount;
        UpdateStaminaPotionsUI();
    }

    public void RemoveStaminaPotion(int amount)
    {
        staminaPotions -= amount;
        UpdateStaminaPotionsUI();
    }

    public bool HasStaminaPotions()
    {
        return staminaPotions > 0;
    }

    private void UpdateTorchesUI()
    {
        torchesText.text = torches.ToString();
    }

    private void UpdateStaminaPotionsUI()
    {
        staminaPotionsText.text = staminaPotions.ToString();
    }

    private void UpdateGroundsUI()
    {
        foreach (var item in grounds)
        {
            switch (item.Ground)
            {
                case Grounds.Dirt:
                    dirtText.text = item.Amount.ToString();
                    break;
                case Grounds.Stone:
                    stoneText.text = item.Amount.ToString();
                    break;
                case Grounds.Bedrock:
                    bedrockText.text = item.Amount.ToString();
                    break;
                case Grounds.DragonStone:
                    dragonStoneText.text = item.Amount.ToString();
                    break;
            }
        }
    }

    private void UpdateValuablesUI()
    {
        foreach (var item in valuables)
        {
            switch (item.Valuable)
            {
                case Valuables.Gold:
                    goldText.text = item.Amount.ToString();
                    break;
                case Valuables.Amethyst:
                    amethystText.text = item.Amount.ToString();
                    break;
                case Valuables.Diamond:
                    diamondText.text = item.Amount.ToString();
                    break;
            }
        }
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