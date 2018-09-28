using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class testLerp : MonoBehaviour {

    Queue<Vector3> queue = new Queue<Vector3>();
    public RawImage rImg;

    private void Update()
    {
        Vector3 vc = new Vector3(Random.Range(50, 100), 0, 0);
        //queue.Enqueue(vc);
        //if (queue.Count > 5)
        //    queue.Dequeue();
        //Vector3 pos = Vector3.zero;
        //foreach (var item in queue)
        //    pos += item;
        //Vector3 averagePos = pos/queue.Count;
        //pos = Vector3.zero;
        rImg.transform.localPosition = LinqUtil.QueueAverage(ref queue,vc,500);
        //rImg.transform.localPosition = Vector3.Lerp(rImg.transform.localPosition, averagePos, Time.deltaTime * 4f);
    }
}
