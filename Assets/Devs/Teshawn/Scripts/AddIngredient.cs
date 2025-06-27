using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    private Vector3 originalPos;

    public Ingredientes ingredientes;
    public string nameOfIngredient;

    public bool isGrabbed;
    [SerializeField] private GrabStatus grabStatus;

    private float destroyDelayTimer = 5f;

    private void Start()
    {
        isGrabbed = true;
        grabStatus = GrabStatus.none;

        nameOfIngredient = ingredientes.nameOfIngredient;
        originalPos = this.transform.position;
    }

    public void SetToGrabbed()
    {
        Invoke("InvokeGrabbed", 1f);
    }

    private void InvokeGrabbed()
    {
        grabStatus = GrabStatus.grabbed;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) == false && grabStatus == GrabStatus.grabbed)
        {
            SetGrabbed(GrabStatus.dropped);
        }

        if (this.transform.position.y < -5)
            this.transform.position = originalPos;

        if (grabStatus == GrabStatus.dropped)
            destroyDelayTimer -= Time.deltaTime;

        if (grabStatus == GrabStatus.grabbed || grabStatus == GrabStatus.picking_up)
            destroyDelayTimer = 5;

        if (destroyDelayTimer <= 0 && grabStatus == GrabStatus.dropped)
        {
            destroyDelayTimer = 0;
            Destroy(this.gameObject);
        }
        //Debug.Log(isGrabbed);
    }

    public void SetGrabbed( GrabStatus status)
    {
        Debug.Log("SetGrabbed: " + status);
        grabStatus = status ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with: " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            if (collision.gameObject.GetComponent<MixingCup>().currentAmount < collision.gameObject.GetComponent<MixingCup>().maxAmount)
            {
                if (collision.gameObject.GetComponent<MixingCup>().drinkToserve != null)
                {
                    collision.gameObject.GetComponent<MixingCup>().currentAmount++;
                    Destroy(this.gameObject);
                }
                else if (!collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Contains(ingredientes))
                {
                    collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Add(ingredientes);
                    collision.gameObject.GetComponent<MixingCup>().ingredientesNames.Add(nameOfIngredient);
                    Destroy(this.gameObject);
                }
                collision.gameObject.GetComponent<MixingCup>().currentAmount++;
                Destroy(this.gameObject);
            }
        }

        if (Input.GetMouseButton(0) == false && grabStatus == GrabStatus.grabbed)
        {
            SetGrabbed(GrabStatus.dropped);
        }
    }
}
