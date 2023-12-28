using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public HealthBase healthBase;

    [Header("Setup")]
    public SOPlayerSetup soPlauerSetup;

    //public Animator animator;

    private float _currentSpeed;
    //private bool _isRunning = false;

    private Animator _currentPlayer;

    [Header("Jump Collision Check")]
    public Collider2D collider2d;
    public float disToGround;
    public float spaceToGround = 0.1f;
    public ParticleSystem jumpVFX;
    public AudioSource audioSourceJump;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;
        }

        _currentPlayer = Instantiate(soPlauerSetup.player, transform);

        if(collider2d != null)
        {
            disToGround = collider2d.bounds.extents.y;
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, disToGround + spaceToGround);
        return Physics2D.Raycast(transform.position, -Vector2.up, disToGround + spaceToGround);
    }

    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlauerSetup.triggerDeath);
    }


    private void Update()
    {
        HandleJump();
        HandleMoviment();
    }

    private void HandleMoviment()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = soPlauerSetup.speedRun;
            _currentPlayer.speed = 2;
        }

        else
        {
            _currentSpeed = soPlauerSetup.speed;
            _currentPlayer.speed = 1;
        }



        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != -1)
            {
                myRigidbody.transform.DOScaleX(-1, soPlauerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlauerSetup.boolRun, true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            if (myRigidbody.transform.localScale.x != 1)
            {
                myRigidbody.transform.DOScaleX(1, soPlauerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlauerSetup.boolRun, true);
        }
        else
        {
            _currentPlayer.SetBool(soPlauerSetup.boolRun, false);
        }


        if(myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += soPlauerSetup.friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= soPlauerSetup.friction;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            myRigidbody.velocity = Vector2.up * soPlauerSetup.forceJump;
            myRigidbody.transform.localScale = Vector2.one;

            DOTween.Kill(myRigidbody.transform);

            audioSourceJump.Play();

            HandleScaleJump();
            PlayJumpVFX();
        }
    }

    private void PlayJumpVFX()
    {
        VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position);
        if (jumpVFX != null) jumpVFX.Play();
    }

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(soPlauerSetup.jumpScaleY, soPlauerSetup.animationDuration). SetLoops(2, LoopType.Yoyo). SetEase(soPlauerSetup.ease);
        myRigidbody.transform.DOScaleX(soPlauerSetup.jumpScaleX, soPlauerSetup.animationDuration). SetLoops(2, LoopType.Yoyo).SetEase(soPlauerSetup.ease);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
