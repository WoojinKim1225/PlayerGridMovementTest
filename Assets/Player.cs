using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public InputActionProperty upProperty, downProperty, leftProperty, rightProperty;
    public InputActionProperty undoProperty;

    public Vector2 lastMoveDirection;

    public Vector2 startTimer;
    private float timer;

    void Awake()
    {
        upProperty.action.Enable();
        downProperty.action.Enable();
        leftProperty.action.Enable();
        rightProperty.action.Enable();
        undoProperty.action.Enable();
        logs.Add(0);
    }

    void OnDestroy()
    {
        upProperty.action.Disable();
        downProperty.action.Disable();
        leftProperty.action.Disable();
        rightProperty.action.Disable();
        undoProperty.action.Disable();
    }

    void OnEnable()
    {
        upProperty.action.started += OnStartUp;
        downProperty.action.started += OnStartDown;
        leftProperty.action.started += OnStartLeft;
        rightProperty.action.started += OnStartRight;
        undoProperty.action.started += OnUndo;

        upProperty.action.canceled += ResetDirection;
        downProperty.action.canceled += ResetDirection;
        leftProperty.action.canceled += ResetDirection;
        rightProperty.action.canceled += ResetDirection;
    }

    void OnDisable()
    {
        upProperty.action.started -= OnStartUp;
        downProperty.action.started -= OnStartDown;
        leftProperty.action.started -= OnStartLeft;
        rightProperty.action.started -= OnStartRight;
        undoProperty.action.started -= OnUndo;

        upProperty.action.canceled -= ResetDirection;
        downProperty.action.canceled -= ResetDirection;
        leftProperty.action.canceled -= ResetDirection;
        rightProperty.action.canceled -= ResetDirection;
    }

    void OnStartUp(InputAction.CallbackContext context) => StartMovement(Vector2.up);

    void OnStartDown(InputAction.CallbackContext context) => StartMovement(Vector2.down);

    void OnStartLeft(InputAction.CallbackContext context) => StartMovement(Vector2.left);

    void OnStartRight(InputAction.CallbackContext context) => StartMovement(Vector2.right);

    void StartMovement(Vector2 direction){
        timer = startTimer.x;
        lastMoveDirection = direction;
        MoveCharacter(lastMoveDirection);
    }

    void ResetDirection(InputAction.CallbackContext context){
        lastMoveDirection = Vector2.zero;
    }

    void OnUndo(InputAction.CallbackContext context) {
        UndoCharacter();
    }

    void Update(){
        if (lastMoveDirection != Vector2.zero){
            timer -= Time.deltaTime;
            if (timer <= 0){
                timer = startTimer.y;
                MoveCharacter(lastMoveDirection);
            }
        }
    }

    public override void MoveCharacter(Vector2 dir){
        transform.position += new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);

        long i;
        if (dir == Vector2.up) {
            i = 0;
        } else if (dir == Vector2.down) {
            i = 1;
        } else if (dir == Vector2.left) {
            i = 2;
        } else {
            i = 3;
        }
        Debug.Log(i);

        logs[logBigNumber] |= (uint)(i << logNumber);
        logNumber += 2;
        if (logNumber == 32) {
            logNumber = 0;
            logBigNumber++;
            logs.Add(0);
        }
    }

    public override void UndoCharacter() {
        if (logNumber == 0 && logBigNumber == 0) return;
        uint i;
        if (logNumber == 0) {
            logBigNumber -= 1;
            logNumber = 30;
            i = (logs[logBigNumber] >> logNumber) & 3;
            logs[logBigNumber] &= ~(uint)(3 << logNumber);
            logs.RemoveAt(logs.Count - 1);
        } else {
            logNumber -= 2;
            i = (logs[logBigNumber] >> logNumber) & 3;
            logs[logBigNumber] &= ~(uint)(3 << logNumber);
        }

        switch (i) {
            case 0:
                transform.position -= new Vector3(0,1,0);
                break;
            case 1:
                transform.position -= new Vector3(0,-1,0);
                break;
            case 2:
                transform.position -= new Vector3(-1,0,0);
                break;
            case 3:
                transform.position -= new Vector3(1,0,0);
                break;
        }
        Debug.Log(i);
    }
}