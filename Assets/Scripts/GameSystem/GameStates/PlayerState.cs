using UnityEngine;

internal class PlayerState : State
{
    private readonly Board _board;

    public PlayerState(Board board)
    {
        _board = board;
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerState Active");
    }

    public override void OnExit()
    {
        
        Debug.Log("MovingStates");
    }
}
