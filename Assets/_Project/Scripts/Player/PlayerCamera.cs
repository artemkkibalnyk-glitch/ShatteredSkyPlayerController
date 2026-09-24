using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    public Transform GetCameraTransfomr() => camTransform;
}
