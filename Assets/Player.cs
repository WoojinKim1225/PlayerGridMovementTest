using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public InputActionProperty upProperty, downProperty, leftProperty, rightProperty; // 방향 입력 액션
    public InputActionProperty undoProperty; // 되돌리기 입력 액션

    public Vector2 lastMoveDirection; // 마지막 이동 방향

    public Vector2 startTimer; // 타이머 초기값, x: 초기값, y: 주기적 값
    public Vector2 undoTimer; // 되돌리기 타이머 초기값, x: 초기값, y: 주기적 값
    private float _time; // 현재 타이머 값
    private float _undoTime; // 되돌리기 타이머 값

    public override void Awake()
    {
        // 입력 액션 활성화
        upProperty.action.Enable();
        downProperty.action.Enable();
        leftProperty.action.Enable();
        rightProperty.action.Enable();
        undoProperty.action.Enable();
        base.Awake();
    }

    void OnDestroy()
    {
        // 입력 액션 비활성화
        upProperty.action.Disable();
        downProperty.action.Disable();
        leftProperty.action.Disable();
        rightProperty.action.Disable();
        undoProperty.action.Disable();
    }

    void OnEnable()
    {
        // 입력 액션 이벤트 연결
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
        // 입력 액션 이벤트 해제
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

    void OnStartUp(InputAction.CallbackContext context) => StartMovement(Vector2.up); // 위로 이동 시작
    void OnStartDown(InputAction.CallbackContext context) => StartMovement(Vector2.down); // 아래로 이동 시작
    void OnStartLeft(InputAction.CallbackContext context) => StartMovement(Vector2.left); // 왼쪽으로 이동 시작
    void OnStartRight(InputAction.CallbackContext context) => StartMovement(Vector2.right); // 오른쪽으로 이동 시작

    void StartMovement(Vector2 direction)
    {
        _time = startTimer.x; // 타이머 초기화
        lastMoveDirection = direction; // 이동 방향 설정
        MoveCharacter(lastMoveDirection); // 캐릭터 이동
    }

    void ResetDirection(InputAction.CallbackContext context)
    {
        lastMoveDirection = Vector2.zero; // 이동 방향 리셋
    }

    void OnUndo(InputAction.CallbackContext context)
    {
        _undoTime = undoTimer.x;
        UndoCharacter(out uint lastMoveNumber); // 캐릭터 되돌리기
    }

    void Update()
    {
        if (lastMoveDirection != Vector2.zero)
        {
            _time -= Time.deltaTime; // 타이머 감소
            if (_time <= 0)
            {
                _time = startTimer.y; // 주기적 타이머 재설정
                MoveCharacter(lastMoveDirection); // 캐릭터 이동
            }
        }

        if (undoProperty.action.ReadValue<float>() > 0.5f)
        {
            _undoTime -= Time.deltaTime; // 타이머 감소
            if (_undoTime <= 0)
            {
                _undoTime = undoTimer.y; // 주기적 타이머 재설정
                UndoCharacter(out uint lastMoveNumber); // 캐릭터 이동
            }
        }
    }

    public override void MoveCharacter(Vector2 dir)
    {
        // 캐릭터 위치 업데이트
        transform.position += new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);

        base.MoveCharacter(dir);
    }

    public override void UndoCharacter(out uint i)
    {
        base.UndoCharacter(out uint num);
        // 캐릭터 위치 되돌리기
        if (num < 4)
            transform.position -= Num2Direction(num);
        i = num;
    }
}