using UnityEngine;

public class UITurnScript : MonoBehaviour
{
    private Camera mainCam;
    private Transform unit;
    Transform worldSpaceCanvas;

    public Vector3 offset;
    private void Start()
    {
        mainCam = FindFirstObjectByType<Camera>();
        unit = transform.parent;
        worldSpaceCanvas = GetComponent<Canvas>().transform;

        transform.SetParent(worldSpaceCanvas);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0, -90, 0));
        transform.position = unit.position + offset;
    }
}
