using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class MenuNetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameInputField;       //�̸� ���� ��
    public TMP_InputField roomCodeInputField;   //�� ��ȣ ���� ��

    public GameObject mainUI;
    public GameObject connectingUI;
    public GameObject playMenuUI;
    public GameObject canvas2;

    private string userName;        //���� �г���
    private string roomCode;        //������ �� �ڵ�
    private Player[] otherPlayerList;    //�濡 �ִ� ���� ����Ʈ

    private PhotonView pv;

    private void Awake()
    {
        //�� ����� �ڵ����� ��ũ ���߱�
        PhotonNetwork.AutomaticallySyncScene = true;

        pv = GetComponent<PhotonView>();
    }

    //ó�� ��Ƽ �÷��� ��ư ���� �� , Master������ ����
    public void Connect()
    {
        if (!PhotonNetwork.ConnectUsingSettings())
        {
            connectingUI.SetActive(false);
            mainUI.SetActive(true);
        }
    }
    // ���� �Ǹ�...
    public override void OnConnectedToMaster()
    {
        //���� �̸� ����
        userName = nameInputField.text;
        PhotonNetwork.LocalPlayer.NickName = userName;

        connectingUI.SetActive(false);
        playMenuUI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        connectingUI.SetActive(false);
        playMenuUI.SetActive(true);

        Ready(false);

        //�� UIŰ��, �� �ڵ� ����
        canvas2.SetActive(true);
        canvas2.transform.Find("Room/RoomCode").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.Name;

        canvas2.transform.Find("Room/Player1/Name").GetComponent<TMP_Text>().text = userName;

        UpdateUser();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        connectingUI.SetActive(false);
        playMenuUI.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateUser();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateUser();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        UpdateUser();
    }
    private void UpdateUser()
    {
        //���� �� �����
        GameObject player2 = canvas2.transform.Find("Room/Player2").gameObject;
        GameObject player3 = canvas2.transform.Find("Room/Player3").gameObject;
        GameObject player4 = canvas2.transform.Find("Room/Player4").gameObject;
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        canvas2.transform.Find("Room/Player2/Ready").gameObject.SetActive(false);
        canvas2.transform.Find("Room/Player3/Ready").gameObject.SetActive(false);
        canvas2.transform.Find("Room/Player4/Ready").gameObject.SetActive(false);

        //������Ʈ
        otherPlayerList = PhotonNetwork.PlayerListOthers;
        if (otherPlayerList.Length > 3)
        {
            Debug.LogError("otherPlayerList is more than 3... ���̰� 3���� ũ�� �ȵ�");
            return;
        }
        
        //������ ��������
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            canvas2.transform.Find("Room/Player1/IsMasterClient").gameObject.SetActive(true);
        else
            canvas2.transform.Find("Room/Player1/IsMasterClient").gameObject.SetActive(false);

        //���� ��ư Ȱ��ȭ
        bool isAllReady = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.TryGetValue("IsReady", out object isReady))
            {
                if (!(bool)isReady)
                    isAllReady = false;
            }
            else
            {
                isAllReady = false;
                break;
            }
        }
        if(PhotonNetwork.LocalPlayer.IsMasterClient && isAllReady && PhotonNetwork.PlayerList.Length > 1)
        {
            canvas2.transform.Find("Room/StartButton").gameObject.SetActive(true);
        }
        else
        {
            canvas2.transform.Find("Room/StartButton").gameObject.SetActive(false);
        }

        //update
        if (otherPlayerList.Length >= 1)
        {
            canvas2.transform.Find("Room/Player2/Name").GetComponent<TMP_Text>().text = otherPlayerList[0].NickName;
            if(otherPlayerList[0].CustomProperties.TryGetValue("IsReady", out object isReady))
            {
                if ((bool)isReady)
                    canvas2.transform.Find("Room/Player2/Ready").gameObject.SetActive(true);
                else
                    canvas2.transform.Find("Room/Player2/Ready").gameObject.SetActive(false);
            }
            if (otherPlayerList[0].IsMasterClient)
                canvas2.transform.Find("Room/Player2/IsMasterClient").gameObject.SetActive(true);
            else
                canvas2.transform.Find("Room/Player2/IsMasterClient").gameObject.SetActive(false);
            player2.SetActive(true);
        }
        if (otherPlayerList.Length >= 2)
        {
            canvas2.transform.Find("Room/Player3/Name").GetComponent<TMP_Text>().text = otherPlayerList[1].NickName;
            if (otherPlayerList[1].CustomProperties.TryGetValue("IsReady", out object isReady))
            {
                if ((bool)isReady)
                    canvas2.transform.Find("Room/Player3/Ready").gameObject.SetActive(true);
                else
                    canvas2.transform.Find("Room/Player3/Ready").gameObject.SetActive(false);
            }
            if (otherPlayerList[1].IsMasterClient)
                canvas2.transform.Find("Room/Player3/IsMasterClient").gameObject.SetActive(true);
            else
                canvas2.transform.Find("Room/Player3/IsMasterClient").gameObject.SetActive(false);
            player3.SetActive(true);
        }
        if (otherPlayerList.Length == 3)
        {
            canvas2.transform.Find("Room/Player4/Name").GetComponent<TMP_Text>().text = otherPlayerList[2].NickName;
            
            if (otherPlayerList[2].CustomProperties.TryGetValue("IsReady", out object isReady))
            {
                if ((bool)isReady)
                    canvas2.transform.Find("Room/Player4/Ready").gameObject.SetActive(true);
                else
                    canvas2.transform.Find("Room/Player4/Ready").gameObject.SetActive(false);
            }
            if (otherPlayerList[2].IsMasterClient)
                canvas2.transform.Find("Room/Player4/IsMasterClient").gameObject.SetActive(true);
            else
                canvas2.transform.Find("Room/Player4/IsMasterClient").gameObject.SetActive(false);
            player4.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        string inputRoomCode = roomCodeInputField.text;
        
        if (!string.IsNullOrEmpty(inputRoomCode))
        {
            playMenuUI.SetActive(false);
            connectingUI.SetActive(true);
            PhotonNetwork.JoinRoom(inputRoomCode);
        }

    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        //roomCode ���� ����
        for(int i = 0; i < 10; i++)
        {
            roomCode = Random.Range(100000, 999999).ToString();
            if (PhotonNetwork.CreateRoom(roomCode, roomOptions))
                break;
        }
    }
    public void Ready(bool isReady)
    {

        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps["IsReady"] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("PlayScene");
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        canvas2.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

}
