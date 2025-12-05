using UnityEngine;

/// <summary>
/// モニターモード内のプレイヤー移動を制御するクラス
/// </summary>
public class MonitorPlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }
    
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // カメラの向きに基づいて移動方向を計算
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 moveDir = camForward * v + camRight * h;
        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isMoving);
        
        if (isMoving)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10f);
        }
        
        // 足音の再生制御
        if (isMoving && !AudioManager.instance.isPlaying("PlayerAFootStep"))
            AudioManager.instance.Play("PlayerAFootStep");
        else if (!isMoving && AudioManager.instance.isPlaying("PlayerAFootStep"))
            AudioManager.instance.Stop("PlayerAFootStep");
    }
}