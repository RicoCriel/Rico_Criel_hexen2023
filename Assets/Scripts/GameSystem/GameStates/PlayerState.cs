using UnityEngine;

internal class PlayerState : State
{
    public override void OnEnter()
    {
        Debug.Log("PlayerState Active");
    }

    public override void OnExit()
    {
        Debug.Log("MovingStates");
    }
}
