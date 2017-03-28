using UnityEngine;
using System.Collections;

public class PositionSync : MonoBehaviour {

    private Vector3 defLocalPosition= new Vector3(-0.213585f, -1f, 0.107f);

    void Update () {
        transform.localPosition = defLocalPosition;

	}
}
