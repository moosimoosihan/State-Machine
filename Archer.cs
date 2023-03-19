using System.Collections;
using UnityEngine;

public class Archer : PlayerAI
{
    // 플레이어와의 거리
    [SerializeField]
    Vector3 playerPos;
    // 플레이어와의 최대 거리
    [SerializeField]
    float playerDis = 10;
        
    [SerializeField]
    Vector2 ranVec;

    void Awake()
    {
        chacter = characterInfo.궁수;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scaner>();
    }

    void Start(){
        playerState = PlayerState.Idle;
        StartCoroutine(StateMachine());
    }
    void Update()
    {
        playerPos = GameManager.instance.player[0].transform.position;
    }
    Vector2 GetRandomPositionAround(Vector2 center, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        return center + randomDirection * Random.Range(-radius, radius);
    }
    public override IEnumerator Idle(){
        if(scanner.nearestTarget==null){
            ranVec = GetRandomPositionAround(playerPos, 2f);
            while(Vector2.Distance(transform.position, ranVec) > 0.5f){
                inputVec = (ranVec-(Vector2)transform.position).normalized;
                yield return null;
            }
            playerState = PlayerState.Search;
        } else {
            playerState = PlayerState.Search;    
        }
    }
    public override IEnumerator MoveToPlayer(){
        while (Vector2.Distance(transform.position, playerPos) > playerDis*0.2f) {
            inputVec = (playerPos - transform.position).normalized;
            yield return null;
        }
        playerState = PlayerState.Search;
    }

    // 적군에게서 멀어지는가 가까워 지는가?
    public override IEnumerator MoveToEnemy(){
        while (Vector2.Distance(transform.position, scanner.nearestTarget.transform.position) < scanner.scanRange*0.2f) {
            inputVec = (scanner.nearestTarget.transform.position + transform.position).normalized;
            yield return null;
        }
        playerState = PlayerState.Search;
    }

    // 행동 양식 (체력비례 혹은 특수한 행동 등등)
    public override IEnumerator Search(){
        yield return null;
        if(Vector2.Distance(transform.position, playerPos)>playerDis){
            playerState = PlayerState.MoveToPlayer;
        } else{
            if(scanner.nearestTarget != null){
                if(Vector2.Distance(transform.position, scanner.nearestTarget.transform.position) < scanner.scanRange*0.2f){
                    playerState = PlayerState.MoveToEnemy;
                } else {
                    playerState = PlayerState.AttackEnemy;
                }
            } else {
                playerState = PlayerState.Idle;
            }
        }
    }
    
    public override IEnumerator Die(){
        yield return null;
        playerDead = true;
    }

    // 자동 공격이라면 없어져도 괜찮을듯 하지만 딜레이 없이 진행된다면 핵처럼 움직이고 캐릭터가 떨림
    public override IEnumerator AttackEnemy(){
        yield return null;
        if(Vector2.Distance(transform.position,scanner.nearestTarget.transform.position)<scanner.scanRange){
            // 공격 범위 안에 있을 경우
            Debug.Log("공격!");
            isAttack = true;
            yield return new WaitForSeconds(attackDelay);
            playerState = PlayerState.Search;
            isAttack = false;
        } else {
            playerState = PlayerState.Search;
        }
    }    
    void LateUpdate()
    {
        inputVec = isAttack ? Vector2.zero : inputVec;
        if(inputVec.x != 0){
            spriter.flipX = inputVec.x > 0;
        } else if(scanner.nearestTarget!=null) {
            spriter.flipX = transform.position.x < scanner.nearestTarget.transform.position.x;
        }
    }
}
