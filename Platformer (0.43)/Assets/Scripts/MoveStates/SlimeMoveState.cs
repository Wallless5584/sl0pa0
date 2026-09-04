using System;
using UnityEngine;

public abstract class SlimeMoveState
{
    public bool IsControlled;
    public sbyte moveX;
    
    public abstract void EnterState(SlimeController slime);

    public abstract void UpdateState(SlimeController slime);

    public abstract void FixedUpdateState(SlimeController slime, Vector2 move_vector);

    public virtual void OnCollisionEnter(SlimeController slime, Collision2D collision, SideData currentSides) { }

    public abstract void ReactToGround(SideData side, SlimeController slime);
}
