using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IState
{
    public StateMachine StateMachine { get; set; }
    void Enter();
    void Suspend();
    void Resume();
    void Update();
    void Exit();
}

