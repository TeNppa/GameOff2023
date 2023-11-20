using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private GameObject goldTooltip;
    [SerializeField] private GameObject gemTooltip;
    [SerializeField] private GameObject dirtTooltip;
    [SerializeField] private GameObject stoneTooltip;
    [SerializeField] private GameObject bedrockTooltip;
    [SerializeField] private GameObject dragonStoneTooltip;
    [SerializeField] private GameObject torchTooltip;
    [SerializeField] private GameObject staminaTooltip;

    public void HoverGoldTooltip() => ActivateTooltip(goldTooltip);
    public void HoverGemTooltip() => ActivateTooltip(gemTooltip);
    public void HoverDirtTooltip() => ActivateTooltip(dirtTooltip);
    public void HoverStoneTooltip() => ActivateTooltip(stoneTooltip);
    public void HoverBedrockTooltip() => ActivateTooltip(bedrockTooltip);
    public void HoverDragonStoneTooltip() => ActivateTooltip(dragonStoneTooltip);
    public void HoverTorchTooltip() => ActivateTooltip(torchTooltip);
    public void HoverStaminaTooltip() => ActivateTooltip(staminaTooltip);

    public void ExitGoldTooltip() => DisableTooltip(goldTooltip);
    public void ExitGemTooltip() => DisableTooltip(gemTooltip);
    public void ExitDirtTooltip() => DisableTooltip(dirtTooltip);
    public void ExitStoneTooltip() => DisableTooltip(stoneTooltip);
    public void ExitBedrockTooltip() => DisableTooltip(bedrockTooltip);
    public void ExitDragonStoneTooltip() => DisableTooltip(dragonStoneTooltip);
    public void ExitTorchTooltip() => DisableTooltip(torchTooltip);
    public void ExitStaminaTooltip() => DisableTooltip(staminaTooltip);


    private void ActivateTooltip(GameObject tooltip)
    {
        tooltip.SetActive(true);
    }


    private void DisableTooltip(GameObject tooltip)
    {
        tooltip.SetActive(false);
    }
}