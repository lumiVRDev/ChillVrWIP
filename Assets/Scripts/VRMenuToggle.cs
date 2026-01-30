using UnityEngine;
using UnityEngine.InputSystem;

public class VRMenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 menuOffset = new Vector3(0f, -0.1f, 1.5f);
    [SerializeField] private InputActionProperty toggleAction;
    [SerializeField] private bool allowKeyboardToggle = true;

    private void OnEnable()
    {
        if (toggleAction.action != null)
        {
            toggleAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleAction.action != null)
        {
            toggleAction.action.Disable();
        }
    }

    private void Update()
    {
        if (WasTogglePressed())
        {
            ToggleMenu();
        }
    }

    private bool WasTogglePressed()
    {
        if (toggleAction.action != null && toggleAction.action.WasPressedThisFrame())
        {
            return true;
        }

        if (allowKeyboardToggle && Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    private void ToggleMenu()
    {
        if (menuRoot == null || cameraTransform == null)
        {
            return;
        }

        var willBeActive = !menuRoot.activeSelf;
        menuRoot.SetActive(willBeActive);

        if (!willBeActive)
        {
            return;
        }

        var targetPosition = cameraTransform.position
            + cameraTransform.right * menuOffset.x
            + cameraTransform.up * menuOffset.y
            + cameraTransform.forward * menuOffset.z;
        menuRoot.transform.position = targetPosition;

        var flatForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        if (flatForward.sqrMagnitude < 0.001f)
        {
            flatForward = cameraTransform.forward;
        }

        menuRoot.transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }
}
