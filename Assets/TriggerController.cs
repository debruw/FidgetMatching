using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerController : MonoBehaviour
{
    public List<Transform> Points;
    int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (index < Points.Count - 1)
            {
                other.transform.parent = Points[index].transform;
                other.transform.localPosition = Vector3.zero;
                other.GetComponent<Collider>().enabled = false;
                index++;
                Debug.Log(other.name);
            }
        }
    }
}
