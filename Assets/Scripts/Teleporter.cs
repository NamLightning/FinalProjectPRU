using UnityEngine;

public class Teleporter : MonoBehaviour
{
    //Điểm đến
    [SerializeField] private Transform destination;


    public Transform GetDestion()
    {
        return destination;
    }
}
