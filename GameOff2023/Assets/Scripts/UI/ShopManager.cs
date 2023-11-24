using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Animator shopAnimator;

    [Header("Shop Pages")]
    [SerializeField] private GameObject toolsPage;
    [SerializeField] private GameObject upgradesPage;
    [SerializeField] private GameObject consumablesPage;
    [SerializeField] private GameObject nextDayPage;

    [Header("Shop pagination")]
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    [SerializeField] private Sprite PaginationNormalSprite;
    [SerializeField] private Sprite PaginationHoverSprite;
    [SerializeField] private float pageTurnDuration = 0.25f;

    [Header("Start New Day")]
    [SerializeField] private Text startNextDayButton;
    [SerializeField] private Color startButtonNormalColor = new Color(1f, 0.843f, 0f);
    [SerializeField] private Color startButtonHoverColor = new Color(1f, 0.647f, 0f);


    // Unity event called from shop next button
    public void CancelHoverPaginationButton()
    {
        // We only have 2 pages and only 1 pagination arrow is ever visible, so this is a quick and dirty way to handle this, will need to be redone if 3rd shop page is added
        nextPageButton.GetComponent<Image>().sprite = PaginationNormalSprite;
        previousPageButton.GetComponent<Image>().sprite = PaginationNormalSprite;
    }


    // Unity event called from shop next button
    public void HoverPaginationButton()
    {
        // We only have 2 pages and only 1 pagination arrow is ever visible, so this is a quick and dirty way to handle this, will need to be redone if 3rd shop page is added
        nextPageButton.GetComponent<Image>().sprite = PaginationHoverSprite;
        previousPageButton.GetComponent<Image>().sprite = PaginationHoverSprite;
    }


    // Unity event called from shop next button
    public void TurnNextPage() => StartCoroutine(TurnNextPageCoroutine());
    private IEnumerator TurnNextPageCoroutine()
    {
        nextPageButton.SetActive(false);
        toolsPage.SetActive(false);
        upgradesPage.SetActive(false);
        CancelHoverPaginationButton();
        shopAnimator.SetTrigger("Turn Next Page");

        yield return new WaitForSeconds(pageTurnDuration);

        previousPageButton.SetActive(true);
        consumablesPage.SetActive(true);
        nextDayPage.SetActive(true);
    }


    // Unity event called from shop previous button
    public void TurnPreviousPage() => StartCoroutine(TurnPreviousPageCoroutine());
    private IEnumerator TurnPreviousPageCoroutine()
    {
        previousPageButton.SetActive(false);
        consumablesPage.SetActive(false);
        nextDayPage.SetActive(false);
        CancelHoverPaginationButton();
        shopAnimator.SetTrigger("Turn Previous Page");

        yield return new WaitForSeconds(pageTurnDuration);

        nextPageButton.SetActive(true);
        toolsPage.SetActive(true);
        upgradesPage.SetActive(true);
    }


    // Unity event called from shop next button
    public void startNewDay()
    {
        this.gameObject.SetActive(false);
    }

    public void HoverstartNewDayButton()
    {
        startNextDayButton.color = startButtonHoverColor;
    }

    public void ExitHoverstartNewDayButton()
    {
        startNextDayButton.color = startButtonNormalColor;
    }
}
