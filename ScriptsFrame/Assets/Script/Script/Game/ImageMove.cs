using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MoveDir
{
    None,
    RightToLeft,
    LeftUpToRightDown,
}

[DisallowMultipleComponent]
public class ImageMove : MonoBehaviour {
    public MoveDir dir = MoveDir.None;

    void LateUpdate() {
        if (dir == MoveDir.RightToLeft)
            MoveRightToLeft();
        else if (dir == MoveDir.LeftUpToRightDown)
            MoveLeftUpToRightDown();
    }

    private void MoveRightToLeft()
    {
        Vector3 target = transform.localPosition + new Vector3(-0.5f, 0, 0);
        transform.localPosition = new Vector3(target.x, target.y, 0);
        if (transform.localPosition.x == -1100f)
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(1100, pos.y, pos.z);
        }
    }

    private void MoveLeftUpToRightDown()
    {
        Vector3 target = transform.localPosition + new Vector3(0.5f, -0.5f, 0);
        transform.localPosition = new Vector3(target.x, target.y, 0);
        if (transform.localPosition.y <= -600f)
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(Random.Range(-1100,-700), 600f, pos.z);
        }
    }
}
