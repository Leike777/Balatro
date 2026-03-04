using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputActionsMap inputActionsMap;
    public MyCsharpClass myCsharpClass = new MyCsharpClass();
    

    private Transform trans;

    public Vector2 moveDir;
    public int moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        
        inputActionsMap = new InputActionsMap();
        inputActionsMap.Enable();
        
        inputActionsMap.PlayerActionsMap.JumpAction.performed += Jump;
        
        
        inputActionsMap.PlayerActionsMap.MoveAction.performed += UpdateMoveDir;
        inputActionsMap.PlayerActionsMap.MoveAction.canceled += UpdateMoveDir;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started)print("JumpStarted");
        if(context.performed)print("JumpPerformed");
        if(context.canceled)print("JumpCanceled");
    }

    public void Move()
    {
        trans.Translate(moveDir * (moveSpeed * Time.fixedDeltaTime), Space.Self);
    }

    public void UpdateMoveDir(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
}
