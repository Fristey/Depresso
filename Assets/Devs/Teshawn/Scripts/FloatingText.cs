using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private AddIngredient ingrediente;
    private CustomerOrder orderOfThisCustomer;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceCanvas;

    public Vector3 offset;
    void Start()
    {
        ingrediente = GetComponentInParent<AddIngredient>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        orderOfThisCustomer = GetComponentInParent<CustomerOrder>();

        
       
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceCanvas = GetComponent<Canvas>().transform;

        transform.SetParent(worldSpaceCanvas);
    }

    private void Update()
    {
        if (orderOfThisCustomer != null)
        {
            textMeshPro.text = orderOfThisCustomer.order.nameOfDrink;
        }else if (ingrediente != null)
        {
            textMeshPro.text = ingrediente.ingredientes.nameOfIngredient;
        }

        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;
    }
}
