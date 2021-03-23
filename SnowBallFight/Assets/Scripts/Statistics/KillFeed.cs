using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class KillFeed : MonoBehaviour
{
    public GameObject killInfoPrefab, KillFeedPanel;
    public Sprite snowballUiImage, randomDeathImage;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SendKillInfo("Player 1", "", "Player 2");
        }
    }

    public void  SendKillInfo(string whoDead, string what, string whoKill)
    {
        StartCoroutine(ShowKillInfo(whoDead, what, whoKill));
    }

    
    private IEnumerator ShowKillInfo(string whoDead, string what, string whoKill )
    {
        //create
        GameObject _killInfo = Instantiate(killInfoPrefab);
        _killInfo.transform.SetParent(KillFeedPanel.transform);
        _killInfo.transform.SetAsFirstSibling();  //set as first child, oldest info is lower in hierarchy 
        _killInfo.transform.localScale = new Vector3(1, 1, 1); // repair scale 
        _killInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = whoKill;

        if(true)
        {
            _killInfo.transform.GetChild(1).GetComponent<Image>().sprite = snowballUiImage;
        }
        _killInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = whoDead;
        yield return new WaitForSeconds(2);


        //change transparency
        CanvasGroup _killInfoCanvasGroup = _killInfo.GetComponent<CanvasGroup>();
        while(_killInfoCanvasGroup.alpha > 0)
        {
            _killInfoCanvasGroup.alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }



        Destroy(_killInfo.gameObject);
    }
}
