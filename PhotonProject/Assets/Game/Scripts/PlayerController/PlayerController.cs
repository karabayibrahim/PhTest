using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Photon.Pun;

public class PlayerController : MonoBehaviour,IPunObservable
{
    [SerializeField] private PlayerType playerType;
    [SerializeField] private PlayerTypeDatas playerTypeDatas;
    [SerializeField] private PlayerData playerData;
    [SerializeField] GameObject modelObject;
    [SerializeField] private PlayerAnimStates playerAnimStates;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject myIcon;
    [SerializeField] private GameObject myCircle;
    [SerializeField] private Enemy targetEnemy;
    private RaycastHit raycastHit;
    private Vector3 walkTarget;
    //private bool isWalkable = false;
    [SerializeField] private bool isAttack = false;
    private bool isRightClick = false;
    void Start()
    {
        PlayerModelEntegred();
        AdjustComponents();
        SpritesStatus();
    }

    public PlayerAnimStates State
    {
        get
        {
            return playerAnimStates;
        }
        set
        {
            if (State == value)
            {
                return;
            }
            playerAnimStates = value;
            OnStateChanged();
        }
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case PlayerAnimStates.IDLE:
                anim.SetBool("Run",false);
                //anim.CrossFade("Idle", 0.05f);
                break;
            case PlayerAnimStates.RUN:
                anim.SetBool("Run",true);
                //anim.CrossFade("Run", 0.05f);
                break;
            case PlayerAnimStates.SHOOT:
                anim.SetBool("Shoot", true);
                break;
            default:
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        myIcon.transform.LookAt(Camera.main.transform);
        if (GetComponent<PhotonView>().IsMine)
        {
            MovementTargetSystem();
            MovementSystem();
            ShootSystem();
        }

    }

    private void PlayerModelEntegred()
    {
        foreach (var item in playerTypeDatas.PlayerDatas)
        {
            if (item.PlayerType == playerType)
            {
                playerData = item;
                modelObject = Instantiate(item.MyModel, transform.position, Quaternion.identity, transform);
            }
        }
    }

    private void MovementTargetSystem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.gameObject.GetComponent<IAttackable>()!=null)
                {
                    targetEnemy = raycastHit.collider.gameObject.GetComponent<Enemy>();
                    walkTarget = new Vector3(raycastHit.point.x,0, raycastHit.point.z);
                    isAttack = true;
                }
                else
                {
                    isAttack = false;
                    walkTarget = raycastHit.point;
                    State = PlayerAnimStates.RUN;
                    anim.SetBool("Shoot", false);
                }
            }
        }

    }

    private void SpritesStatus()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            myIcon.SetActive(true);
        }
        else
        {
            myCircle.SetActive(true);
        }
    }

    private void MovementSystem()
    {
        if (State==PlayerAnimStates.RUN)
        {
            transform.position = Vector3.MoveTowards(transform.position, walkTarget, playerData.WalkSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(raycastHit.point.x,0, raycastHit.point.z));
            if (Vector3.Distance(transform.position, walkTarget) <= 0f)
            {
                State = PlayerAnimStates.IDLE;
            }
        }
    }

    private void ShootControl()
    {
        if (Vector3.Distance(transform.position, walkTarget) >= 15f)
        {
            State = PlayerAnimStates.RUN;
        }
        else if (Vector3.Distance(transform.position, walkTarget) <15f&&Vector3.Distance(transform.position, walkTarget)>5f)
        {
            State = PlayerAnimStates.SHOOT;
            transform.position = Vector3.MoveTowards(transform.position, walkTarget, playerData.WalkSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(raycastHit.point.x, 0, raycastHit.point.z));
        }
        else if (Vector3.Distance(transform.position, walkTarget) <= 5f)
        {
            State = PlayerAnimStates.IDLE;
        }
    }

    private void ShootSystem()
    {
        if (isAttack)
        {
            ShootControl();
            anim.runtimeAnimatorController = playerData.ShootController;
        }
    }

    private void AdjustComponents()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = playerData.AnimController;
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Veriyi stream ediyorum
        if (stream.IsWriting)
        {
            stream.SendNext(State);
        }
        else
        {
            State = (PlayerAnimStates)stream.ReceiveNext();
        }
    }

    
}
