using UnityEngine;

public class UnlockRecipe : MonoBehaviour
{
    private OrderManager orderManager;
    private CamSwapManager camSwapManager;
    [Header("recepe buttons")]
    [SerializeField] private GameObject prevButton, nextButton, currentRecepe, previousRecepe;

    [SerializeField] private GameObject appleCin, candyCane, cattechino, cherryBlos, coffee, cosmos, espressoDepres, fakeCoffee, iceCoffee, lavander, pumpkinSpice, rosecarda;
    [SerializeField] private Recipes appleCinR, candyCaneR, cattechinoR, cherryBlosR, coffeeR, cosmosR, espressoDepreR, fakeCoffeeR, iceCoffeeR, lavanderR, pumpkinSpiceR, rosecardaR;

    public GameObject unlockRecipeMenu, mainRecipeBookMenu;
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
        }
    }

    public void UnlockMoreRecipes()
    {
        if (!orderManager.possibleDrinks.Contains(recipe))
        {
            orderManager.possibleDrinks.Add(recipe);
            mainRecipeBookMenu.SetActive(true);
            unlockRecipeMenu.SetActive(false);
        }
        else
        {
            Debug.Log("you have this drink");
        }
    }

    public void NextPage()
    {
        if (previousRecepe != null)
            previousRecepe.SetActive(false);

        if (currentRecepe == appleCin)
        {
            previousRecepe = appleCin;
            previousRecepe.SetActive(false);
            currentRecepe = candyCane;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == candyCane)
        {
            previousRecepe = candyCane;
            previousRecepe.SetActive(false);
            currentRecepe = cattechino;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == cattechino)
        {
            previousRecepe = cattechino;
            previousRecepe.SetActive(false);
            currentRecepe = cherryBlos;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == cherryBlos)
        {
            previousRecepe = cherryBlos;
            previousRecepe.SetActive(false);
            currentRecepe = coffee;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == coffee)
        {
            previousRecepe = coffee;
            previousRecepe.SetActive(false);
            currentRecepe = cosmos;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == cosmos)
        {
            previousRecepe = cosmos;
            previousRecepe.SetActive(false);
            currentRecepe = espressoDepres;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == espressoDepres)
        {
            previousRecepe = espressoDepres;
            previousRecepe.SetActive(false);
            currentRecepe = fakeCoffee;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == fakeCoffee)
        {
            previousRecepe = fakeCoffee;
            previousRecepe.SetActive(false);
            currentRecepe = pumpkinSpice;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == pumpkinSpice)
        {
            previousRecepe = pumpkinSpice;
            previousRecepe.SetActive(false);
            currentRecepe = lavander;
            currentRecepe.SetActive(true);
        }
        else if (currentRecepe == lavander)
        {
            previousRecepe = lavander;
            previousRecepe.SetActive(false);
            currentRecepe = appleCin;
            currentRecepe.SetActive(true);
        }
    }

    public void PrevPage()
    {
        if (currentRecepe != null)
            currentRecepe.SetActive(false);

        if (currentRecepe == appleCin)
        {
            previousRecepe = pumpkinSpice;
            previousRecepe.SetActive(true);
            currentRecepe = lavander;
            currentRecepe.SetActive(false);
        }
        else if (currentRecepe == candyCane)
        {
            previousRecepe = lavander;
            previousRecepe.SetActive(true);
            currentRecepe = appleCin;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == cattechino)
        {
            previousRecepe = appleCin;
            previousRecepe.SetActive(true);
            currentRecepe = candyCane;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == cherryBlos)
        {
            previousRecepe = candyCane;
            previousRecepe.SetActive(true);
            currentRecepe = cattechino;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == coffee)
        {
            previousRecepe = cattechino;
            previousRecepe.SetActive(true);
            currentRecepe = cherryBlos;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == cosmos)
        {
            previousRecepe = cherryBlos;
            previousRecepe.SetActive(true);
            currentRecepe = coffee;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == espressoDepres)
        {
            previousRecepe = coffee;
            previousRecepe.SetActive(true);
            currentRecepe = cosmos;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == fakeCoffee)
        {
            previousRecepe = cosmos;
            previousRecepe.SetActive(true);
            currentRecepe = espressoDepres;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == pumpkinSpice)
        {
            previousRecepe = espressoDepres;
            previousRecepe.SetActive(true);
            currentRecepe = fakeCoffee;
            currentRecepe.SetActive(false);

        }
        else if (currentRecepe == lavander)
        {
            previousRecepe = fakeCoffee;
            previousRecepe.SetActive(true);
            currentRecepe = pumpkinSpice;
            currentRecepe.SetActive(false);

        }
    }

    #region Unlock Functions
    public void UnlockAppleCin()
    {
        recipe = appleCinR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);
        Debug.Log(mainRecipeBookMenu);
    }

    public void UnlockCandyCane()
    {
        recipe = candyCaneR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockCattachino()
    {
        recipe = cattechinoR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockCherryBlos()
    {
        recipe = cattechinoR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockCoffee()
    {
        recipe = coffeeR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockCosmos()
    {
        recipe = cosmosR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);


    }

    public void UnlockEpressoDepress()
    {
        recipe = espressoDepreR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);


    }

    public void UnlockFakeCoffee()
    {
        recipe = fakeCoffeeR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);
        Debug.Log(mainRecipeBookMenu);


    }

    public void UnlockIceCoffee()
    {
        recipe = iceCoffeeR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);


    }

    public void UnlockLavander()
    {
        recipe = lavanderR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockPumpkinSpice()
    {
        recipe = pumpkinSpiceR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }

    public void UnlockRosecarda()
    {
        recipe = rosecardaR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);

        Debug.Log(mainRecipeBookMenu);

    }
    #endregion

    public void Deny()
    {
        mainRecipeBookMenu.SetActive(true);
        unlockRecipeMenu.SetActive(false);

    }
}
