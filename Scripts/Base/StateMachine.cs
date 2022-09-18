public class StateMachine 
{
    public State State => m_state;

    protected State m_state;
    protected State nextState;

    protected State idleState = new IdleState();

    public StateMachine() => SetIdleState();

    public void SetIdleState() => nextState = idleState;

    public void Update() 
    {
        if (nextState != null)
            SetState(nextState);

        if (m_state != null)
            m_state.OnUpdate();
    }

    public void ChangeState(State newState) 
    {
        if (newState != null)
            nextState = newState;
    }

    private void SetState(State newState) 
    {
        nextState = null;

        if (m_state != null)
            m_state.OnExit();

        m_state = newState;
        m_state.OnEnter(this);
    }
}
