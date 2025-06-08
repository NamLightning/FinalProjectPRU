using System.Xml.Serialization;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    //Vị trí dịch chuyển
    private GameObject currentTeleporter;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(currentTeleporter != null)
            {

                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestion().position;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Teleporter"))
        {
            currentTeleporter = null;
        }
    }
}
