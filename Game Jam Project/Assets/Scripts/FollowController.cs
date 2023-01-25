using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        CameraController.instance.followTransform = this.transform;
    }
}
