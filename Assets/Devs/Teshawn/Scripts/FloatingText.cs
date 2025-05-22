using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private AddIngredient ingrediente;
    private CustomerOrder orderOfThisCustomer;
    private MixingCup mixingCup;

    public Camera mainCam;
    private Transform unit;
    Transform worldSpaceCanvas;

    public Vector3 offset;
    void Start()
    {
        ingrediente = GetComponentInParent<AddIngredient>();
        orderOfThisCustomer = GetComponentInParent<CustomerOrder>();
        mixingCup = GetComponentInParent<MixingCup>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();


        mainCam = FindFirstObjectByType<Camera>();
        unit = transform.parent;
        worldSpaceCanvas = GetComponent<Canvas>().transform;

        transform.SetParent(worldSpaceCanvas);
    }

    private void Update()
    {
        if (orderOfThisCustomer != null)
        {
            textMeshPro.text = string.Join("\n", orderOfThisCustomer.orderText);

        }
        else if (ingrediente != null)
        {
            textMeshPro.text = ingrediente.ingredientes.nameOfIngredient;
        }
        else if (mixingCup != null)
        {
            if (mixingCup.ingredientesNames.Count > 0) 
            {
                textMeshPro.text = string.Join("\n", mixingCup.ingredientesNames);
            }
            else if(mixingCup.drinkToserve != null)
            {
                textMeshPro.text = mixingCup.drinkToserve.nameOfDrink;
            }
            
        }
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;
    }
}
