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

    [Header("recepe buttons")]
    [SerializeField] private GameObject prevButton, nextButton, unlockButton;

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
        mainRecipeBookMenu.SetActive(true);
    }

    private void Update()
    {

        if (camSwapManager.isLookingAtBook)
        {
            this.gameObject.SetActive(true);

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
            }
        }

        Display();
    }

    public void NextPage()
    {

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
                recipe = pageRecipes[recipeIndex];
                pricetag.text = price.ToString();
            }
            else
            {
                unlockButton.SetActive(false);
                
            }
        }
        price = recipe.price;
    }

    public void Unlock()
    {
        if(recipe != null)
        {
            orderManager.possibleDrinks.Add(recipe);
        }
    }
}
