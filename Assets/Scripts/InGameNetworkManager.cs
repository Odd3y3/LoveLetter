using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InGameNetworkManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    private PunTurnManager turnManager;
    public InGameManager inGameManager;

    private Player[] playerList;    //플레이어 리스트( 턴 순서)
    private int myTurn = -1;             //자기 턴 넘버 ( 0 ~ 3 )
    private bool initSettingEnd = false;    //초기 세팅 끝나면 true


    public GameObject player;
    public GameObject leftPlayer;
    public GameObject topPlayer;
    public GameObject rightPlayer;

    public GameObject loadingImage;

    private const int DECK_LENGTH = 16;

    //게임 정보 (InitRoundRPC에서 초기화)
    private int[] cardDeck;
    private int cardDeckPointer;

    void Awake()
    {
        turnManager = GetComponent<PunTurnManager>();
        turnManager.TurnManagerListener = this;
        if(inGameManager == null)
            inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();

        //playerList 초기화
        playerList = PhotonNetwork.PlayerList;

        Load();

    }

    private void Load()
    {
        //씬 시작하고 로딩
        //OnPlayerPropertiesUpdate에서 받기
        Hashtable loadProperty = new Hashtable();
        loadProperty.Add("IsLoadEnd", true);
        PhotonNetwork.SetPlayerCustomProperties(loadProperty);
    }


    // 모든 플레이어가 로딩이 끝나면 실행
    private void InitRoomSetting()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //초기 설정
            photonView.RPC("InitRPC", RpcTarget.All);

            //라운드 설정(덱 설정, 한 장씩 주기)
            int[] cardDeck = GetShuffledCards();
            photonView.RPC("InitRoundRPC", RpcTarget.All, cardDeck, 0);
            
        }
    }

    //처음 덱 섞을 때
    private int[] GetShuffledCards()
    {
        System.Random random = new System.Random();
        //총 16장
        int[] cards = { 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8 };
        cards = cards.OrderBy(x => random.Next()).ToArray();
        
        Debug.Log(string.Join(' ',cards));

        return cards;
    }

    //로딩 끝나는거 알리면서, 초기 세팅하라고 RPC로 보내기
    [PunRPC]
    public void InitRPC()
    {
        int playerLength = playerList.Length;

        //myTurn 초기화
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i] == PhotonNetwork.LocalPlayer)
                myTurn = i;
        }

        //---------자리 설정 -------------
        int leftPlayerTurn = (myTurn + 1) % 4;
        int topPlayerTurn = (myTurn + 2) % 4;
        int rightPlayerTurn = (myTurn + 3) % 4;

        if (leftPlayerTurn < playerLength)
        {
            leftPlayer.transform.Find("Name").GetComponent<TextMeshPro>().text = playerList[leftPlayerTurn].NickName;
            leftPlayer.SetActive(true);
        }
        if (topPlayerTurn < playerLength)
        {
            topPlayer.transform.Find("Name").GetComponent<TextMeshPro>().text = playerList[topPlayerTurn].NickName;
            topPlayer.SetActive(true);
        }
        if (rightPlayerTurn < playerLength)
        {
            rightPlayer.transform.Find("Name").GetComponent<TextMeshPro>().text = playerList[rightPlayerTurn].NickName;
            rightPlayer.SetActive(true);
        }
        player.transform.Find("Name").GetComponent<TextMeshPro>().text = playerList[myTurn].NickName;

        //초기 설정 끝
        initSettingEnd = true;

    }

    //라운드 시작 함수( 초기 설정 끝나고 or 라운드 끝나고 다시 시작할 때? )
    [PunRPC]
    public void InitRoundRPC(int[] cardDeck, int startTurn)
    {
        // !! InitRPC 실행 전에 실행 될 수 있음 !! 

        //덱 설정(로컬)
        this.cardDeck = cardDeck;

        //InitRPC가 끝날 때 까지 기다리기위해 Coroutine 사용
        StartCoroutine(InitRoundRPCCoroutine());
    }

    IEnumerator InitRoundRPCCoroutine()
    {
        //초기 설정이 끝나지 않았으면 1초 대기후 다시 실행..  ( 
        if (!initSettingEnd)
            yield return new WaitForSeconds(1.0f);

        //카드 나눠주고(덱 포인터 증가), 자기도 한장 받기
        inGameManager.DrawCard(cardDeck[myTurn]);
        cardDeckPointer = playerList.Length;

        //3초후 라운드 시작
        yield return new WaitForSeconds(2.0f);

        if(PhotonNetwork.IsMasterClient)
            turnManager.BeginTurn();

    }





    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //처음 모든 플레이어가 로드 되면.....
        if (PhotonNetwork.IsMasterClient)
        {
            bool isAllLoad = true;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("IsLoadEnd"))
                {
                    isAllLoad = false;
                }
                else if((bool)player.CustomProperties["IsLoadEnd"] == false)
                {
                    isAllLoad = false;
                }
            }
            if (isAllLoad)
            {
                InitRoomSetting();
            }

        }
    }


    /// <summary>
    /// 덱에 카드가 몇장 남아있는지
    /// </summary>
    /// <returns></returns>
    public int GetRemainCardsCountInDeck()
    {
        int count = DECK_LENGTH - cardDeckPointer + 1;
        return count > 0 ? count : 0;
    }



    /// <summary>
    /// 네트워크로 보낼 정보를 받아 object로 변환 후 turnManager로 정보 보내줌.
    /// 그리고 턴이 넘어감
    /// </summary>
    /// <param name="cardNum"></param>
    /// <param name="targetPlayerNum"></param>
    /// <param name="optionNum"></param>
    public void SendMove(int cardNum, int targetPlayerNum, int optionNum)
    {
        object packetInfo = new int[3]{ cardNum, targetPlayerNum, optionNum};
        turnManager.SendMove(packetInfo, true);
    }


    /// <summary>
    /// Network에서 받은 Object를 Packet으로 변환해주는 함수
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    private Packet ObjectToPacket(object move)
    {
        int[] moveInfo = (int[])move;

        Packet packet = new Packet(moveInfo[0], moveInfo[1], moveInfo[2]);

        return packet;
    }







    //-----------------------------------------------------------------------------------------------------
    //------------- TurnManager Callback 함수 ----------------
    



    /// <summary>
    /// 턴이 시작 될 때 호출되는 함수
    /// </summary>
    /// <param name="turn"></param>
    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins : " + turn);


        //현재 턴 계산
        int turnNum = (turn - 1) % playerList.Length;
        if(turnNum == myTurn)                           //자기 턴일 때
        {
            inGameManager.DrawCard(cardDeck[cardDeckPointer]);   //카드 뽑기
            cardDeckPointer++;                          //덱 포인터 증가
            inGameManager.MyTurnStart();
        }
        else
        {
            cardDeckPointer++;
        }

        //남은 덱이 0장 일 때
        if(GetRemainCardsCountInDeck() <= 0)
        {
            //패 공개.
            // 큰사람이 이김.
            //마지막 스코어 보드(결과 창) 열고,
            //종료.
        }
    }

    //현재 사용되지 않음.
    public void OnTurnCompleted(int turn)
    {
        Debug.Log("OnTurnCompleted : " + turn);
    }

    //현재 사용되지 않음.
    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerMove : " + player.NickName + " " + turn + " " + move);
    }


    /// <summary>
    /// SendMove로 정보를 보내면서 턴을 넘길 때 호출 되는 함수.
    /// Object 정보를 받아서 사용
    /// </summary>
    /// <param name="player"></param>
    /// <param name="turn"></param>
    /// <param name="move"></param>
    public void OnPlayerFinished(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerFinished : " + player.NickName + " " + turn);
        
        //move -> Packet으로 변환
        Packet moveInfo = ObjectToPacket(move);

        Debug.Log($"MoveInfo : {moveInfo.CardNum} {moveInfo.TargetPlayerNum} {moveInfo.OptionNum}");



        //턴 넘김
        if(PhotonNetwork.LocalPlayer == player)
            turnManager.BeginTurn();
    }


    //현재 사용되지 않음.
    public void OnTurnTimeEnds(int turn)
    {
        //타임 아웃 설정
        //( 일단은 시간 제한 0으로 무시 )
    }
}


/// <summary>
/// 턴이 넘어 갈 때(플레이어가 카드를 선택 할 때), 네트워크로 주고받을 데이터 정보
/// </summary>
struct Packet
{
    public int CardNum { get; }
    public int TargetPlayerNum { get; }
    public int OptionNum { get; }

    public Packet(int cardNum, int targetPlayerNum, int optionNum)
    {
        CardNum = cardNum;
        TargetPlayerNum = targetPlayerNum;
        OptionNum = optionNum;
    }
}