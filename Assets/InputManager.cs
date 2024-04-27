using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public static InputManager instance;

    public PlayerInput playerInput;
    private InputAction menuOpenClose;


    [Header("----- Input Actions -----")]
    public InputActionReference pause;
    public InputActionReference move;
    public InputActionReference look;
    public InputActionReference fire;
    public InputActionReference interact;
    public InputActionReference jump;
    public InputActionReference crouch;
    public InputActionReference sprint;
    private InputAction navigationAction;

    [Header("---- Input Directions -----")]
    public Vector2 moveDirection;
    public Vector2 lookDirection;
    public Vector2 navigationInput { get; set; }

    public bool MenuOpenCloseInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool CrouchInput {  get; private set; }
    public bool SprintPressedInput { get; private set; }
    public bool SprintReleasedInput { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        MenuOpenCloseInput = pause.action.WasPressedThisFrame();
        FireInput = fire.action.IsPressed();
        InteractInput = interact.action.WasPressedThisFrame();
        JumpInput = jump.action.WasPressedThisFrame();
        CrouchInput = crouch.action.WasPressedThisFrame();
        SprintPressedInput = sprint.action.WasPressedThisFrame();
        SprintReleasedInput = sprint.action.WasReleasedThisFrame();

        moveDirection = move.action.ReadValue<Vector2>();
        lookDirection = look.action.ReadValue<Vector2>();
    }

}
