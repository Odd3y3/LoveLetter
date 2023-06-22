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

    private Player[] playerList;    //�÷��̾� ����Ʈ( �� ����)
    private int myTurn = -1;             //�ڱ� �� �ѹ� ( 0 ~ 3 )
    private bool initSettingEnd = false;    //�ʱ� ���� ������ true


    public GameObject player;
    public GameObject leftPlayer;
    public GameObject topPlayer;
    public GameObject rightPlayer;

    public GameObject loadingImage;

    private const int DECK_LENGTH = 16;

    //���� ���� (InitRoundRPC���� �ʱ�ȭ)
    private int[] cardDeck;
    private int cardDeckPointer;

    void Awake()
    {
        turnManager = GetComponent<PunTurnManager>();
        turnManager.TurnManagerListener = this;
        if(inGameManager == null)
            inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();

        //playerList �ʱ�ȭ
        playerList = PhotonNetwork.PlayerList;

        Load();

    }

    private void Load()
    {
        //�� �����ϰ� �ε�
        //OnPlayerPropertiesUpdate���� �ޱ�
        Hashtable loadProperty = new Hashtable();
        loadProperty.Add("IsLoadEnd", true);
        PhotonNetwork.SetPlayerCustomProperties(loadProperty);
    }


    // ��� �÷��̾ �ε��� ������ ����
    private void InitRoomSetting()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //�ʱ� ����
            photonView.RPC("InitRPC", RpcTarget.All);

            //���� ����(�� ����, �� �徿 �ֱ�)
            int[] cardDeck = GetShuffledCards();
            photonView.RPC("InitRoundRPC", RpcTarget.All, cardDeck, 0);
            
        }
    }

    //ó�� �� ���� ��
    private int[] GetShuffledCards()
    {
        System.Random random = new System.Random();
        //�� 16��
        int[] cards = { 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8 };
        cards = cards.OrderBy(x => random.Next()).ToArray();
        
        Debug.Log(string.Join(' ',cards));

        return cards;
    }

    //�ε� �����°� �˸��鼭, �ʱ� �����϶�� RPC�� ������
    [PunRPC]
    public void InitRPC()
    {
        int playerLength = playerList.Length;

        //myTurn �ʱ�ȭ
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i] == PhotonNetwork.LocalPlayer)
                myTurn = i;
        }

        //---------�ڸ� ���� -------------
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

        //�ʱ� ���� ��
        initSettingEnd = true;

    }

    //���� ���� �Լ�( �ʱ� ���� ������ or ���� ������ �ٽ� ������ ��? )
    [PunRPC]
    public void InitRoundRPC(int[] cardDeck, int startTurn)
    {
        // !! InitRPC ���� ���� ���� �� �� ���� !! 

        //�� ����(����)
        this.cardDeck = cardDeck;

        //InitRPC�� ���� �� ���� ��ٸ������� Coroutine ���
        StartCoroutine(InitRoundRPCCoroutine());
    }

    IEnumerator InitRoundRPCCoroutine()
    {
        //�ʱ� ������ ������ �ʾ����� 1�� ����� �ٽ� ����..  ( 
        if (!initSettingEnd)
            yield return new WaitForSeconds(1.0f);

        //ī�� �����ְ�(�� ������ ����), �ڱ⵵ ���� �ޱ�
        inGameManager.DrawCard(cardDeck[myTurn]);
        cardDeckPointer = playerList.Length;

        //3���� ���� ����
        yield return new WaitForSeconds(2.0f);

        if(PhotonNetwork.IsMasterClient)
            turnManager.BeginTurn();

    }





    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //ó�� ��� �÷��̾ �ε� �Ǹ�.....
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


    public int GetRemainCardsCountInDeck()
    {
        int count = DECK_LENGTH - cardDeckPointer + 1;
        return count > 0 ? count : 0;
    }




    

    public void SendMove(int cardNum)       // �׽�Ʈ �� ( ��ư�� ���� )
    {
        turnManager.SendMove(cardNum, true);
    }
    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins : " + turn);
        Debug.Log("myTurn : " + myTurn);
        Debug.Log("playerList.Length : " + playerList.Length);

        int turnNum = (turn - 1) % playerList.Length;

        if(turnNum == myTurn)                           //�ڱ� ���� ��
        {
            inGameManager.DrawCard(cardDeck[cardDeckPointer]);   //ī�� �̱�
            cardDeckPointer++;                          //�� ������ ����
            inGameManager.MyTurnStart();
        }
        else
        {
            cardDeckPointer++;
        }

        //���� ���� 0�� �� ��
        if(GetRemainCardsCountInDeck() <= 0)
        {
            //�� ����.
            // ū����� �̱�.
            //������ ���ھ� ����(��� â) ����,
            //����.
        }
    }

    public void OnTurnCompleted(int turn)
    {
        Debug.Log("OnTurnCompleted : " + turn);

        
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerMove : " + player.NickName + " " + turn + " " + move);

    }

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerFinished : " + player.NickName + " " + turn + " " + move);
        if(PhotonNetwork.LocalPlayer == player)
            turnManager.BeginTurn();
    }

    public void OnTurnTimeEnds(int turn)
    {
        //Ÿ�� �ƿ� ����
        //( �ϴ��� �ð� ���� 0���� ���� )
    }
}
