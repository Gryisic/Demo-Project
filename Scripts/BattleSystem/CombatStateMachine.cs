public class CombatStateMachine : StateMachine
{
    public void AttackPerformed() 
    {
        MeleeBaseState state = (MeleeBaseState)m_state;

        state.AttackPerformed();
    }
}
