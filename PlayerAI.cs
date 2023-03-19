using System.Collections;

public abstract class PlayerAI : Player
{
    public enum PlayerState{ Idle, MoveToPlayer, AttackEnemy, MoveToEnemy, Die, Search }
    public PlayerState playerState;
    
    public IEnumerator StateMachine(){
        while(!playerDead){
            switch(playerState){
                case PlayerState.Idle:
                yield return StartCoroutine(Idle());
                break;
                case PlayerState.MoveToPlayer:
                yield return StartCoroutine(MoveToPlayer());
                break;
                case PlayerState.AttackEnemy:
                yield return StartCoroutine(AttackEnemy());
                break;
                case PlayerState.MoveToEnemy:
                yield return StartCoroutine(MoveToEnemy());
                break;
                case PlayerState.Die:
                yield return StartCoroutine(Die());
                break;
                case PlayerState.Search:
                yield return StartCoroutine(Search());
                break;
            }
        }
    }
    public abstract IEnumerator Idle();
    public abstract IEnumerator MoveToPlayer();
    public abstract IEnumerator AttackEnemy();
    public abstract IEnumerator MoveToEnemy();
    public abstract IEnumerator Die();
    public abstract IEnumerator Search();
}
