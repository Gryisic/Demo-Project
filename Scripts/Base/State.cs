public class State 
{
    protected StateMachine stateMachine;

    protected float timer;

    public virtual void OnEnter(StateMachine stateMachine) => this.stateMachine = stateMachine;
    public virtual void OnUpdate() => timer += UnityEngine.Time.deltaTime;
    public virtual void OnExit() { }
}
