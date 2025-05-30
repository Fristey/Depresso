using UnityEngine;

public class BonsaiCutting : MonoBehaviour
{

    [SerializeField] private GameObject bonsaiLeaf;
    [SerializeField] GameObject bonsaiSpawnPoint;
    [SerializeField] Animator animation;
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
        if(other.gameObject.CompareTag("Scizzor"))
        {
            animation.SetTrigger("Snip");
            Instantiate(bonsaiLeaf, bonsaiSpawnPoint.transform.position, Quaternion.identity);

        }
        animation.SetTrigger("idle");
    }
}
