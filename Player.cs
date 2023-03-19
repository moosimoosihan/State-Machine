using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 자신의 캐릭터
    public enum characterInfo { 전사, 궁수, 마법사, 힐러 }
    public characterInfo chacter;

    [Header("플레이어 컨트롤 설정")]
    public Vector2 inputVec;
    public float speed;
    public Rigidbody2D rigid;

    [Header("이미지 혹은 에니메이션 설정")]
    public SpriteRenderer spriter;
    public Animator anim;
    public RuntimeAnimatorController[] animCharacter;

    [Header("플레이어 상태 설정")]
    public bool playerDead = false;
    // 무속성, 불, 물, 흙, 바람, 전기, 정신, 빛, 어둠 등 상태 추가 예정

    [Header("플레이어 공격 설정")]
    public bool isAttack;
    public float attackDelay;
    public Scaner scanner;
    // 자신이 AI인가?
    public bool playerAI = false;


    void Awake()
    {
        //초기화
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scaner>();

        //사운드
        //audioSource = GetComponent<AudioSource>(); 
    }
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    public void OnMove(InputValue value)
    {
        if(playerAI) return;

        inputVec = value.Get<Vector2>();
    }
    void LateUpdate()
    {
        if(playerAI) return;
        
        if(inputVec.x != 0){
            spriter.flipX = inputVec.x > 0;
        }
    }
}
