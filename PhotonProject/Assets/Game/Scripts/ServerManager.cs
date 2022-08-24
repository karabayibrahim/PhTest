using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private float readyCount;
    private bool isServer = false;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        ReferansObject.Instance.UIManager.StartButton.onClick.AddListener(StartLoby);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartLoby()
    {
        if (isServer)
        {
            PhotonNetwork.JoinLobby();
            ReferansObject.Instance.UIManager.StartPanel.SetActive(false);
            ReferansObject.Instance.UIManager.LobyPanel.SetActive(true);
        }
        
    }

    public override void OnConnectedToMaster()
    {
        isServer = true;
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine(CountDown());
    }

    public override void OnJoinedRoom()
    {
        StartRoom();
    }
    private IEnumerator CountDown()
    {
        
        while (readyCount>0)
        {
            yield return new WaitForSeconds(1f);
            readyCount--;
            ReferansObject.Instance.UIManager.Count.text = readyCount.ToString();
        }
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        yield return null;
       
    }

    private void StartRoom()
    {
        ReferansObject.Instance.UIManager.LobyPanel.SetActive(false);
        PhotonNetwork.Instantiate("Player", new Vector3(0,0.5f,0), Quaternion.identity,0,null);
    }
}
