using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public TileEffect givenEffect;

    public List<uint> logs = new List<uint>();
    public int logNumber = 0;
    public int logBigNumber = 0;

    public abstract void MoveCharacter(Vector2 dir);
    public abstract void UndoCharacter();
}