using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class CubeController : MonoBehaviour
{
    public GameObject Arrow;
    public float Speed;
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Arrow.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, Speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -Speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-Speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Speed * Time.deltaTime, 0, 0);
            }
        }
       
    }

    
}
