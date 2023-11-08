using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomTileRule : RuleTile
{
    // Overrides the default neighbor matching behavior
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        // Matches only empty spaces if required by the rule (neighbor == 2)
        if (other == null)
        {
            return neighbor == 2;
        }

        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.This: return true;
            case TilingRuleOutput.Neighbor.NotThis: return false;
        }

        return base.RuleMatch(neighbor, other);
    }
}