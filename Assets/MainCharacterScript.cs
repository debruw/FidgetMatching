using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BansheeGz;
using BansheeGz.BGSpline.Components;

public class MainCharacterScript : MonoBehaviour
{
    public Vector3 translation;

    private Touch touch;

    public CharacterController controller;
    public float xSpeed;
    public GameObject ChildObject;
    public Animator PlayerAnimator;
    public BGCcTrs bcgTRS;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 move = controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;

            controller.Move(move * Time.deltaTime);
            controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -2, 2), 0, 0);
        }

#elif UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                move += controller.transform.right * Input.GetAxis("Mouse X") * xSpeed;
            }
            controller.Move(move * Time.deltaTime);
            controller.transform.localPosition = new Vector3(Mathf.Clamp(controller.transform.localPosition.x, -2, 2), 0, 0);
        }
#endif
    }
}
