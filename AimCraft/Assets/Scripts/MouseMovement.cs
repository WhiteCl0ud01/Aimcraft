using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public InputActionReference lookRef;
    public Transform pitchTarget;
    public Transform yawTarget;

    private Vector2 rot;

    public float sens;
    public float dpi = 800;
    private void OnEnable()
    {
        lookRef.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        lookRef.action.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        float inchesPer360 = (14.2857f * 360f) / (sens * dpi);

        float conversion = 360f / (inchesPer360 * dpi * sens);
        rot += lookRef.action.ReadValue<Vector2>() * sens * conversion;

        float eplison = .01f;
        rot.x %= 360f;
        rot.y = Mathf.Clamp(rot.y, -90f + eplison, 90f - eplison);

        pitchTarget.localRotation = Quaternion.Euler(-rot.y, pitchTarget.localEulerAngles.y, pitchTarget.localEulerAngles.z);
        yawTarget.localRotation = Quaternion.Euler(yawTarget.localEulerAngles.x, rot.x, yawTarget.localEulerAngles.z);
    }







}
