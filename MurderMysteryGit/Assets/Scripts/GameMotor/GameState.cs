using UnityEngine;

public abstract class GameState
{
    public GameMotor motor;
    public int myInt;
    public virtual void Construct() { }
    public virtual void Destruct() { }
    public virtual void UpdateState() { }
}
