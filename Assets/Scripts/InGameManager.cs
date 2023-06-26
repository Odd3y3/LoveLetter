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

    private void FixedUpdate()
    {
        remainCardCount.text = inGameNetworkManager.GetRemainCardsCountInDeck().ToString();
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

    public void MyTurnStart()
    {
        hand.SetActive(false);
        selectCardUI.gameObject.SetActive(true);
        selectCardUI.SetCard(myHandCards[0], myHandCards[1]);
    }

    /// <summary>
    /// ���� ī�带 ����ϴ� �Լ�
    /// </summary>
    /// <param name="isLeft">���� ī�带 ��� �� �� true</param>
    public void UseCard(bool isLeft)
    {
        int remainCardNum;
        int useCardNum;
        if (isLeft)
        {
            remainCardNum = myHandCards[1];
            useCardNum = myHandCards[0];
        }
        else
        {
            remainCardNum = myHandCards[0];
            useCardNum = myHandCards[1];
        }
        myHandCards[0] = 0;
        myHandCards[1] = 0;
        Destroy(handCardInstance); 
        DrawCard(remainCardNum);
        hand.SetActive(true);

        //ī�带 ����ϰ� ( Card class ����� )***

        //CardOptionOrUseCard(useCardNum);

        //NetworkManager���� �����ϰ�, �� �Ѿ
        //(�������� �ִ� ī���� ���, �ٷ� �Ѿ�� �ʰ�, ������ �ɼ��� ������ �� ������ ���� �Ѱܾ���.)

        //inGameNetworkManager.SendMove(useCardNum);        SelectCardUI����..??????
    }

}
