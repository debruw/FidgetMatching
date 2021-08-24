using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BansheeGz;
using BansheeGz.BGSpline.Components;
using UnityEngine.UI;

public class MainCharacterScript : MonoBehaviour
{
    public Vector3 translation;

    private Touch touch;

    public GameObject childObject;
    public float xSpeed, zSpeed = 15;
    public GameObject ChildObject;
    public Animator PlayerAnimator;
    public BGCcTrs bcgTRS;

    public List<GameObject> CollectedItems;
    public GameObject camera;
    public Transform cameraPosition;

    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                GameManager.Instance.Tutorial1.SetActive(false);
            }
            translation = new Vector3(Input.GetAxis("Mouse X"), 0, 0) * Time.deltaTime * xSpeed;
            childObject.transform.Translate(translation, Space.Self);
            childObject.transform.localPosition = new Vector3(Mathf.Clamp(childObject.transform.localPosition.x, -3.5f, 3.5f), 0, 0);
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                GameManager.Instance.Tutorial1.SetActive(false);
            }
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                childObject.transform.localPosition = new Vector3(Mathf.Clamp(childObject.transform.localPosition.x + touch.deltaPosition.x * Time.deltaTime * (xSpeed / 20), -3.5f, 3.5f), 0, 0);
            }
            else if (touch.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
#endif
        slider.value = bcgTRS.DistanceRatio;
    }

    public GameObject[] willBeClose;
    public GameObject[] willBeOpen;
    private Vector3 move;
    private Vector2 firstPressPos;

    public void MoveCameraToMatchingView()
    {
        camera.transform.DOMove(cameraPosition.position, 1);
        camera.transform.DORotate(cameraPosition.eulerAngles, 1).OnComplete(() =>
        {
            camera.transform.parent = null;
            foreach (var item in willBeClose)
            {
                item.SetActive(false);
            }
            foreach (var item in willBeOpen)
            {
                item.SetActive(true);
            }
            StartCoroutine(WaitAndDeactivate());
            if (GameManager.Instance.currentLevel == 1)
            {
                GameManager.Instance.Tutorial2.SetActive(true);
            }
        });
        GameManager.Instance.itemManager.SetCanvasObjects(CollectedItems);
    }

    IEnumerator WaitAndDeactivate()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
