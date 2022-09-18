public class EnemyStateMachine : StateMachine
{
    public EnemyStateMachine(EnemyBaseState idleState) => base.idleState = idleState;
}
