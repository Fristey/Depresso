using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro, amountTextpro;
    private AddIngredient ingrediente;
    private CustomerOrder orderOfThisCustomer;
    private MixingCup mixingCup;


    void Start()
    {
        ingrediente = GetComponentInParent<AddIngredient>();
        orderOfThisCustomer = GetComponentInParent<CustomerOrder>();
        mixingCup = GetComponentInParent<MixingCup>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
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
            amountTextpro.text = mixingCup.currentAmount.ToString() + "/100";
            amountTextpro.text.Trim();
            if (mixingCup.ingredientesNames.Count > 0)
            {
                textMeshPro.text = string.Join("\n", mixingCup.ingredientesNames);

            }
            else if (mixingCup.drinkToserve != null)
            {
                textMeshPro.text = mixingCup.drinkName;
            }
            else
            {
                textMeshPro.text = string.Join("\n", mixingCup.ingredientesNames);
            }
        }
    }
}
