using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraShot initialShot;

    private CameraShot activeShot;
    private Vector3 transitionStartPosition;
    private Quaternion transitionStartRotation;
    private float transitionElapsed;
    private bool isTransitioning;

    private void Start()
    {
        if (initialShot != null)
        {
            SetActiveShot(initialShot, true);
        }
    }

    private void LateUpdate()
    {
        if (activeShot == null)
        {
            return;
        }

        Vector3 targetPosition = activeShot.Position;
        Quaternion targetRotation = activeShot.GetRotation(player);

        if (!isTransitioning)
        {
            transform.SetPositionAndRotation(targetPosition, targetRotation);
            return;
        }

        transitionElapsed += Time.deltaTime;
        float duration = activeShot.TransitionDuration;
        float normalizedTime = duration > 0f
            ? Mathf.Clamp01(transitionElapsed / duration)
            : 1f;
        float smoothedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);

        transform.SetPositionAndRotation(
            Vector3.Lerp(transitionStartPosition, targetPosition, smoothedTime),
            Quaternion.Slerp(transitionStartRotation, targetRotation, smoothedTime));

        if (normalizedTime >= 1f)
        {
            isTransitioning = false;
        }
    }

    public void ActivateShot(CameraShot shot)
    {
        if (shot == null || shot == activeShot)
        {
            return;
        }

        SetActiveShot(shot, false);
    }

    public bool IsPlayer(Collider other)
    {
        if (player == null || other == null)
        {
            return false;
        }

        Transform otherTransform = other.transform;
        return otherTransform == player || otherTransform.IsChildOf(player);
    }

    private void SetActiveShot(CameraShot shot, bool forceCut)
    {
        activeShot = shot;

        if (forceCut
            || shot.TransitionMode == CameraTransitionMode.Cut
            || shot.TransitionDuration <= 0f)
        {
            isTransitioning = false;
            transform.SetPositionAndRotation(shot.Position, shot.GetRotation(player));
            return;
        }

        transitionStartPosition = transform.position;
        transitionStartRotation = transform.rotation;
        transitionElapsed = 0f;
        isTransitioning = true;
    }
}
