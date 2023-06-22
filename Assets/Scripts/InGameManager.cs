using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private InGameNetworkManager inGameNetworkManager;

    public SelectCardUI selectCardUI;
    public TextMeshProUGUI remainCardCount;
    
    public GameObject hand;

    private GameObject handCardInstance;

    const int MAX_CARD_COUNT = 8;

    GameObject[] cardSources;     //ī�� ���ҽ� ( 0���� �޸� )
    Vector3 cardDeckPos = new Vector3(-1f, 0.5f, 0f);            //ī�� �� ��ġ
    Vector3 myHandCardPos = new Vector3(0f, -3.5f, 0f);         //���տ� �ִ� ī�� ��ġ

    int[] myHandCards;              //���տ� �ִ� ī��, ���ٸ� 0

    void Awake()
    {
        inGameNetworkManager = FindFirstObjectByType<InGameNetworkManager>();
        if (inGameNetworkManager == null)
            Debug.LogWarning("inGameNetworkManager is null.");

        myHandCards = new int[2];
        cardSources = new GameObject[MAX_CARD_COUNT + 1];

        //���ҽ����� ī�� �ε�
        cardSources[0] = Resources.Load<GameObject>("Cards/BackCard");
        for(int i = 1; i <= MAX_CARD_COUNT; i++)
            cardSources[i] = Resources.Load<GameObject>("Cards/Card" + i);

    }

    void Update()
    {
        //(�׽�Ʈ ��)
        if(Input.GetKeyDown(KeyCode.E))
        {
            DrawCardAll();
        }
    }

    private void FixedUpdate()
    {
        remainCardCount.text = inGameNetworkManager.GetRemainCardsCountInDeck().ToString();
    }

    void DrawCardAll()
    {
        //���� ī�� �̴� �ִϸ��̼�
        GameObject leftCard = Instantiate(cardSources[0], cardDeckPos, Quaternion.identity);
        Vector3 leftCardEndPos = new Vector3(-12.0f, cardDeckPos.y, 0f);
        StartCoroutine(MoveCard(leftCard.transform, leftCardEndPos));
    }

    IEnumerator MoveCard(Transform card, Vector3 endPos)
    {
        float t = 0f;
        //1�� �ڿ� ī��(�޸�) ������Ʈ ����
        while (t < 1f)
        {
            t += Time.deltaTime;
            card.position = Vector3.Lerp(card.position, endPos, 0.01f);
            yield return null;
        }
        Destroy(card.gameObject);
    }


    public void DrawCard(int cardNum)
    {
        if (myHandCards[0] == 0)
        {
            myHandCards[0] = cardNum;
            handCardInstance = Instantiate(cardSources[cardNum], Vector3.zero, Quaternion.identity);
            handCardInstance.transform.SetParent(hand.transform, false);
        }
        else
        {
            myHandCards[1] = cardNum;
        }
    }

    private void DrawCardAnim(PlayerDir playerDir)
    {

    }

    public void MyTurnStart()
    {
        hand.SetActive(false);
        selectCardUI.gameObject.SetActive(true);
        selectCardUI.SetCard(myHandCards[0], myHandCards[1]);
    }

    public void UseCardLeft()
    {
        int remainCardNum = myHandCards[1];
        myHandCards[0] = 0;
        myHandCards[1] = 0;
        Destroy(handCardInstance); 
        DrawCard(remainCardNum);
        hand.SetActive(true);
    }

    public void UseCardRight()
    {
        int remainCardNum = myHandCards[0];
        myHandCards[0] = 0;
        myHandCards[1] = 0;
        Destroy(handCardInstance);
        DrawCard(remainCardNum);
        hand.SetActive(true);
    }
}

enum PlayerDir
{
    Left,
    Right,
    Top,
    Me
}