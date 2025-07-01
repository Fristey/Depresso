using UnityEngine;

public class UnlockRecipe : MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;
    private CamSwapManager camSwapManager;
    [Header("recepe buttons")]
    [SerializeField] private GameObject prevButton, nextButton, currentRecepe, previousRecepe;

    [SerializeField] private GameObject appleCin, candyCane, cattechino, cherryBlos, coffee, cosmos, espressoDepres, iceCoffee, lavander, pumpkinSpice, rosecarda;
    [SerializeField] private Recipes appleCinR, candyCaneR, cattechinoR, cherryBlosR, coffeeR, cosmosR, espressoDepreR, iceCoffeeR, lavanderR, pumpkinSpiceR, rosecardaR;
    [SerializeField] private Material appleCinM, candyCaneM, cattechinoM, cherryBlosM, coffeeM, cosmosM, espressoDepreM, iceCoffeeM, lavanderM, pumpkinSpiceM, rosecardaM;

    public GameObject book;
    public GameObject unlockRecipeMenu, mainRecipeBookMenu;
    public Recipes recipe;

    void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
        camSwapManager = FindFirstObjectByType<CamSwapManager>();
        mainRecipeBookMenu.SetActive(true);
        book.GetComponent<MeshRenderer>().material = appleCinM;
    }

    private void Update()
    {
        if (camSwapManager.isLookingAtBook)
        {
            this.gameObject.SetActive(true);
        }
        Debug.Log("book: "+ book + "mesh: " + book.GetComponent<MeshRenderer>().material + "current recipe: " + appleCinM);

        if(Input.GetKey(KeyCode.Escape))
        {
            camSwapManager.isLookingAtBook = false;
        }
        if (currentRecepe == appleCin)
        {
         
            book.GetComponent<MeshRenderer>().material = appleCinM;
        }
        else if (currentRecepe == candyCane)
        {
          
            book.GetComponent<MeshRenderer>().material = candyCaneM;

        }
        else if (currentRecepe == cattechino)
        {
        
            book.GetComponent<MeshRenderer>().material = cattechinoM;
        }
        else if (currentRecepe == cherryBlos)
        {
           
            book.GetComponent<MeshRenderer>().material = cherryBlosM;
        }
        else if (currentRecepe == coffee)
        {

            book.GetComponent<MeshRenderer>().material = coffeeM;
        }
        else if (currentRecepe == cosmos)
        {
           
            book.GetComponent<MeshRenderer>().material = cosmosM;
        }
        else if (currentRecepe == espressoDepres)
        {
          
            book.GetComponent<MeshRenderer>().material = espressoDepreM;
        }
        else if (currentRecepe == pumpkinSpice)
        {
           
            book.GetComponent<MeshRenderer>().material = pumpkinSpiceM;
        }
        else if (currentRecepe == lavander)
        {
          
            book.GetComponent<MeshRenderer>().material = lavanderM;
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
            book.GetComponent<MeshRenderer>().material = appleCinM;
        }
        else if (currentRecepe == candyCane)
        {
            previousRecepe = candyCane;
            previousRecepe.SetActive(false);
            currentRecepe = cattechino;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = candyCaneM;

        }
        else if (currentRecepe == cattechino)
        {
            previousRecepe = cattechino;
            previousRecepe.SetActive(false);
            currentRecepe = cherryBlos;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = cattechinoM;
        }
        else if (currentRecepe == cherryBlos)
        {
            previousRecepe = cherryBlos;
            previousRecepe.SetActive(false);
            currentRecepe = coffee;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = cherryBlosM;
        }
        else if (currentRecepe == coffee)
        {
            previousRecepe = coffee;
            previousRecepe.SetActive(false);
            currentRecepe = cosmos;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = coffeeM;
        }
        else if (currentRecepe == cosmos)
        {
            previousRecepe = cosmos;
            previousRecepe.SetActive(false);
            currentRecepe = espressoDepres;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = cosmosM;
        }
        else if (currentRecepe == espressoDepres)
        {
            previousRecepe = espressoDepres;
            previousRecepe.SetActive(false);
            currentRecepe = pumpkinSpice;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = espressoDepreM;
        }
        else if (currentRecepe == pumpkinSpice)
        {
            previousRecepe = pumpkinSpice;
            previousRecepe.SetActive(false);
            currentRecepe = lavander;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = pumpkinSpiceM;
        }
        else if (currentRecepe == lavander)
        {
            previousRecepe = lavander;
            previousRecepe.SetActive(false);
            currentRecepe = appleCin;
            currentRecepe.SetActive(true);
            book.GetComponent<MeshRenderer>().material = lavanderM;
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
        else if (currentRecepe == pumpkinSpice)
        {
            previousRecepe = cosmos;
            previousRecepe.SetActive(true);
            currentRecepe = espressoDepres;
            currentRecepe.SetActive(false);
        }
        else if (currentRecepe == lavander)
        {
            previousRecepe = espressoDepres;
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
    }

    public void UnlockCandyCane()
    {
        recipe = candyCaneR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);
    }

    public void UnlockCattachino()
    {
        recipe = cattechinoR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);


    }

    public void UnlockCherryBlos()
    {
        recipe = cherryBlosR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);


    }

    public void UnlockCoffee()
    {
        recipe = coffeeR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);


    }

    public void UnlockCosmos()
    {
        recipe = cosmosR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);



    }

    public void UnlockEpressoDepress()
    {
        recipe = espressoDepreR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);



    }

    public void UnlockIceCoffee()
    {
        recipe = iceCoffeeR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);



    }

    public void UnlockLavander()
    {
        recipe = lavanderR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);


    }

    public void UnlockPumpkinSpice()
    {
        recipe = pumpkinSpiceR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);


    }

    public void UnlockRosecarda()
    {
        recipe = rosecardaR;
        unlockRecipeMenu.SetActive(true);
        mainRecipeBookMenu.SetActive(false);
    }
    #endregion

    public void Deny()
    {
        mainRecipeBookMenu.SetActive(true);
        unlockRecipeMenu.SetActive(false);
    }
}
