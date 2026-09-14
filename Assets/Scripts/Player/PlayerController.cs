using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("moveSpeed")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float turnSpeed = 120f;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        float moveInput = 0f;
        float turnInput = 0f;
        bool isSprinting = false;

        if (keyboard != null)
        {
            moveInput = (keyboard.wKey.isPressed ? 1f : 0f)
                - (keyboard.sKey.isPressed ? 1f : 0f);
            turnInput = (keyboard.dKey.isPressed ? 1f : 0f)
                - (keyboard.aKey.isPressed ? 1f : 0f);
            isSprinting = keyboard.leftShiftKey.isPressed
                || keyboard.rightShiftKey.isPressed;
        }

        float steeringDirection = moveInput < 0f ? -1f : 1f;
        float effectiveTurnInput = turnInput * steeringDirection;

        transform.Rotate(Vector3.up, effectiveTurnInput * turnSpeed * Time.deltaTime);

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        float currentMoveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 velocity = transform.forward * (moveInput * currentMoveSpeed);
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }
}
