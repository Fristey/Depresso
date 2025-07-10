using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockRecipe : MonoBehaviour
{
    private int unlockPrice;

    [SerializeField] private OrderManager orderManager;
    [SerializeField] private CurrencyManager currencyManager;
    private CamSwapManager camSwapManager;

    [SerializeField] private TextMeshProUGUI pricetag;
    [SerializeField] private GameObject priceIcon;

    [SerializeField] private Color purchasableColor;
    [SerializeField] private Color unPurchasableColor;

    [SerializeField] private GameObject unlockButton;

    [SerializeField] private List<Material> pageMats;
    [SerializeField] private List<Recipes> pageRecipes;

    [SerializeField] private int recipeIndex;
    [SerializeField] private int price;

    public GameObject book;
    public GameObject mainRecipeBookMenu;
    public Recipes recipe;

    void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
        camSwapManager = FindFirstObjectByType<CamSwapManager>();
        mainRecipeBookMenu.SetActive(false);
    }

    private void Update()
    {

        if (camSwapManager.isLookingAtBook)
        {
            this.gameObject.SetActive(true);
            mainRecipeBookMenu.SetActive(true);

            if (Input.GetKeyDown(KeyCode.A))
            {
                PrevPage();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                NextPage();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                camSwapManager.isLookingAtBook = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                mainRecipeBookMenu.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.P)) 
            {
                Unlock();
            }
        }

        Display();
    }

    public void NextPage()
    {
        Debug.Log("right");
        if (recipeIndex == pageRecipes.Count - 1)
        {
            recipeIndex = 0;
        }
        else
        {
            recipeIndex++;
        }
    }

    public void PrevPage()
    {
        Debug.Log("left");
        if (recipeIndex == 0)
        {
            recipeIndex = pageRecipes.Count - 1;
        }
        else
        {
            recipeIndex--;
        }
    }

    public void Display()
    {
        for (int i = 0; i < pageRecipes.Count; i++)
        {
            book.GetComponent<MeshRenderer>().material = pageMats[recipeIndex];
        }

        for (int i = 0; i < orderManager.possibleDrinks.Count; i++)
        {
            if (!orderManager.possibleDrinks.Contains(pageRecipes[recipeIndex]))
            {
                unlockButton.SetActive(true);
                pricetag.gameObject.SetActive(true);
                priceIcon.SetActive(true);

                recipe = pageRecipes[recipeIndex];
                pricetag.text = price.ToString();
                if(price <= currencyManager.playerCurrency)
                {
                    pricetag.color = purchasableColor;
                } else
                {
                    pricetag.color = unPurchasableColor;
                }
            }
            else
            {
                unlockButton.SetActive(false);
                pricetag.gameObject.SetActive(false);
                priceIcon.SetActive(false);
            }
        }

        price = recipe.price;
    }

    public void Unlock()
    {
        if(recipe != null && currencyManager.playerCurrency >= price)
        {
            currencyManager.RemoveCurrency(price);
            orderManager.possibleDrinks.Add(recipe);
        }
    }
}
