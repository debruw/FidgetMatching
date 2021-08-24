using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TapticPlugin;

public class TriggerController : MonoBehaviour
{
    public MainCharacterScript mainCharacterScript;
    public List<Transform> Points;
    public GameObject popEffect, poffEffect;
    GameObject destroyThis;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            SoundManager.Instance.playSound(SoundManager.GameSounds.Collect);
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Light);

            Instantiate(poffEffect, other.transform.position, Quaternion.identity);
            other.transform.parent.transform.parent = Points[Random.Range(0, Points.Count)].transform;
            other.transform.parent.transform.localPosition = Vector3.zero;
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent.GetComponent<Animator>().enabled = false;
            other.transform.localScale = Vector3.one;

            bool isMatched = false;
            foreach (GameObject item in mainCharacterScript.CollectedItems)
            {
                if (other.GetComponent<Item>().me == item.GetComponent<Item>().me)
                {
                    isMatched = true;
                    destroyThis = item;
                    item.transform.parent.transform.DOMoveY(item.transform.parent.transform.position.y + 4, .5f).OnComplete(() =>
                    {
                        Instantiate(popEffect, item.transform.parent.transform.position, Quaternion.identity);
                        mainCharacterScript.CollectedItems.Remove(destroyThis);
                        Destroy(item.transform.parent.gameObject);
                    });
                    other.transform.parent.transform.DOMoveY(other.transform.parent.transform.position.y + 4, .5f).OnComplete(() =>
                    {
                        Instantiate(popEffect, other.transform.parent.transform.position, Quaternion.identity);
                        Destroy(other.gameObject);
                    });
                    SoundManager.Instance.playSound(SoundManager.GameSounds.Matched);
                    if (PlayerPrefs.GetInt("VIBRATION") == 1)
                        TapticManager.Impact(ImpactFeedback.Light);
                    break;
                }
            }
            if (!isMatched)
            {
                mainCharacterScript.CollectedItems.Add(other.gameObject);
            }
            isMatched = false;
        }
        else if (other.CompareTag("FinishLine"))
        {
            mainCharacterScript.PlayerAnimator.SetTrigger("Sit");
            mainCharacterScript.MoveCameraToMatchingView();
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Medium);

            GameManager.Instance.isGameOver = true;
            mainCharacterScript.PlayerAnimator.SetTrigger("Fall");
            foreach (GameObject item in mainCharacterScript.CollectedItems)
            {
                item.transform.parent.parent = null;
                item.transform.parent.gameObject.AddComponent<Rigidbody>().velocity = new Vector3(Random.Range(5, 10), Random.Range(5, 10), Random.Range(5, 10));
            }
            StartCoroutine(GameManager.Instance.WaitAndGameLose());
        }
    }
}
