using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public TileEffect givenEffect;

    [SerializeField] private uint[] logs;
    [SerializeField] private int logBitLocation = 0; // 로그에서의 비트 위치, 32가 주기
    [SerializeField] private int logIndex = 0; // 로그 배열의 인덱스
    private const int InitialCapacity = 8; // 초기 배열 크기

    private Vector3[] _directions = new Vector3[4] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    // 로그 배열을 초기화하고 기본 항목을 추가
    public virtual void Awake()
    {
        logs = new uint[InitialCapacity]; // 배열을 초기화
        logs[0] = 0; // 첫 번째 로그 항목 초기화
    }

    public virtual void MoveCharacter(Vector2 dir)
    {
        // 방향을 숫자로 전환
        uint i = Direction2Num(dir);

        // 로그에 이동 방향 저장 (2비트씩 저장)
        logs[logIndex] |= (uint)(i << logBitLocation); // 현재 위치(logBitLocation)에 이동 방향(i)을 비트 연산으로 저장
        logBitLocation += 2; // 2비트씩 이동 방향을 저장하기 때문에 logBitLocation을 2만큼 증가
        if (logBitLocation == 32)
        {
            logBitLocation = 0; // 비트 위치를 초기화
            logIndex++; // 로그 배열의 다음 인덱스로 이동
            if (logIndex >= logs.Length)
            {
                // 배열 크기 늘리기: 현재 크기의 두 배로 증가
                ResizeArray();
            }
        }
    }

    private void ResizeArray()
    {
        // 현재 배열 크기를 저장
        int newSize = logs.Length;

        // 배열 크기를 2배로 증가
        while (logIndex >= newSize)
        {
            newSize *= 2;
        }

        // 새 배열 생성 및 복사
        uint[] newLogs = new uint[newSize];
        System.Array.Copy(logs, newLogs, logs.Length);
        logs = newLogs;
    }

    public virtual void UndoCharacter(out uint i)
    {
        if (logBitLocation == 0 && logIndex == 0)
        {
            i = 4; // 되돌릴 이동이 없는 경우
            return;
        }

        if (logBitLocation == 0)
        {
            logIndex -= 1; // 이전 로그 인덱스로 이동
            logBitLocation = 30; // 로그 비트 위치를 30으로 설정 (2비트씩 16개가 32비트를 채우므로 30부터 시작)
        }
        else
        {
            logBitLocation -= 2; // 2비트 이전으로 이동
        }

        i = (logs[logIndex] >> logBitLocation) & 3; // 현재 비트 위치의 2비트를 읽음
        logs[logIndex] &= ~(uint)(3 << logBitLocation); // 해당 2비트를 지움

        return;
    }

    public Vector3 Num2Direction(uint input)
    {
        return _directions[input]; // 숫자를 방향으로 변환
    }

    public uint Direction2Num(Vector3 direction)
    {
        switch (direction.y)
        {
            case 1: return 0; // 위쪽
            case -1: return 1; // 아래쪽
        }
        switch (direction.x)
        {
            case -1: return 2; // 왼쪽
            case 1: return 3; // 오른쪽
        }
        return 0;
    }
}
