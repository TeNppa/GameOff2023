using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    [SerializeField]
    private List<ValuablesInventory> valuables;
    [SerializeField]
    private List<GroundsInventory> grounds;

    public void AddGround(int ground, int amount)
    {
        Debug.Log("Trying to add " + (Grounds)ground + ", amount: " + amount );
        grounds.FirstOrDefault(g => (int)g.Ground == ground)!.Amount += amount;
    }
    public void UseGround(int ground, int amount)
    {
    }
    public void AddValuable(int valuable, int amount)
    {
        valuables.FirstOrDefault(v => (int)v.Valuable == valuable)!.Amount += amount;
    }
    public void UseValuable(int valuable, int amount)
    {
        
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
