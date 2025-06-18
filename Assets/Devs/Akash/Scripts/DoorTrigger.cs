using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.ClickedDoor();
    }
}
