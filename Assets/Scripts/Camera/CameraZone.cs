using UnityEngine;

public sealed class CameraZone : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CameraShot cameraShot;

    private void OnTriggerEnter(Collider other)
    {
        if (cameraManager != null && cameraManager.IsPlayer(other))
        {
            cameraManager.ActivateShot(cameraShot);
        }
    }
}
