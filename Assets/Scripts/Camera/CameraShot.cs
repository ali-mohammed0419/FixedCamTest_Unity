using UnityEngine;

public enum CameraTransitionMode
{
    Cut,
    Smooth
}

public sealed class CameraShot : MonoBehaviour
{
    [SerializeField] private bool tracking;
    [SerializeField] private CameraTransitionMode transitionMode = CameraTransitionMode.Cut;
    [SerializeField, Min(0f)] private float transitionDuration = 0.5f;

    public CameraTransitionMode TransitionMode => transitionMode;
    public float TransitionDuration => transitionDuration;
    public Vector3 Position => transform.position;

    public Quaternion GetRotation(Transform player)
    {
        if (!tracking || player == null)
        {
            return transform.rotation;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        if (directionToPlayer.sqrMagnitude <= Mathf.Epsilon)
        {
            return transform.rotation;
        }

        return Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }
}
