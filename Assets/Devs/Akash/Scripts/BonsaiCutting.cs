using UnityEngine;

public class BonsaiCutting : MonoBehaviour
{

    [SerializeField] private GameObject bonsaiLeaf;
    [SerializeField] GameObject bonsaiSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ontrigger");
        if(other.gameObject.CompareTag("Cup"))
        {
            Instantiate(bonsaiLeaf, bonsaiSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
