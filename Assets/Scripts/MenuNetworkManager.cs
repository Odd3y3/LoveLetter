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
    public TMP_InputField nameInputField;       //이름 쓰는 곳
    public TMP_InputField roomCodeInputField;   //방 번호 쓰는 곳

    public GameObject mainUI;
    public GameObject connectingUI;
    public GameObject playMenuUI;
    public GameObject canvas2;

    private string userName;        //유저 닉네임
    private string roomCode;        //입장한 방 코드
    private Player[] otherPlayerList;    //방에 있는 유저 리스트

    private PhotonView pv;

    private void Awake()
    {
        //씬 변경시 자동으로 싱크 맞추기
        PhotonNetwork.AutomaticallySyncScene = true;

        pv = GetComponent<PhotonView>();
    }

    //처음 멀티 플레이 버튼 누를 때 , Master서버랑 연결
    public void Connect()
    {
        if (!PhotonNetwork.ConnectUsingSettings())
        {
            connectingUI.SetActive(false);
            mainUI.SetActive(true);
        }
    }
    // 연결 되면...
    public override void OnConnectedToMaster()
    {
        //유저 이름 지정
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

        //방 UI키기, 룸 코드 설정
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
        //기존 꺼 지우기
        GameObject player2 = canvas2.transform.Find("Room/Player2").gameObject;
        GameObject player3 = canvas2.transform.Find("Room/Player3").gameObject;
        GameObject player4 = canvas2.transform.Find("Room/Player4").gameObject;
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        canvas2.transform.Find("Room/Player2/Ready").gameObject.SetActive(false);
        canvas2.transform.Find("Room/Player3/Ready").gameObject.SetActive(false);
        canvas2.transform.Find("Room/Player4/Ready").gameObject.SetActive(false);

        //업데이트
        otherPlayerList = PhotonNetwork.PlayerListOthers;
        if (otherPlayerList.Length > 3)
        {
            Debug.LogError("otherPlayerList is more than 3... 길이가 3보다 크면 안됨");
            return;
        }
        
        //본인이 방장인지
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            canvas2.transform.Find("Room/Player1/IsMasterClient").gameObject.SetActive(true);
        else
            canvas2.transform.Find("Room/Player1/IsMasterClient").gameObject.SetActive(false);

        //시작 버튼 활성화
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
        //roomCode 랜덤 생성
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
