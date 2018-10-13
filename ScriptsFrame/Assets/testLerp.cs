using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using CommandTerminal;
using System;

public class testLerp : MonoBehaviour {

    Queue<Vector3> queue = new Queue<Vector3>();
    public RawImage rImg;
    public Transform cube;

    public List<Texture> list = new List<Texture>();

    private IEnumerator Wait()
    {
        while (index < 200)
        {
            Debug.LogError(index);
            yield return 0;
        }
 
        Debug.LogError(1111111);
    }

    private void Start()
    {
        Debug.Log(tips);
        Loom.RunAsync(() =>
        {
            Debug.Log(1);
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log(2);
            });
        });
        Debug.Log(3);
        //StartCoroutine(Wait());
    }

    private void Update()
    {
        Vector3 pos = rImg.transform.localPosition;
        //rImg.transform.localPosition = new Vector3(pos.x, pos.y + Mathf.PerlinNoise(Time.time, 0) + pos.y, 0);

        rImg.color = new Color(MathHelpr.PerlinNoise(PerlinNoise.Right), Mathf.PerlinNoise(0, Time.time), Mathf.PerlinNoise(Time.time, Time.time));

        //cube.transform.position = new Vector3(0, Mathf.PerlinNoise(Time.time, 0) * 3 + 1, 0);
        //AA();

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    BB();
        //    //SMPostEffectsTransition smtranstion = GameObject.Instantiate(Resources.Load<GameObject>("Transitions/SMPostEffectsTransition")).GetComponent<SMPostEffectsTransition>();
        //}

        //Vector3 vc = new Vector3(Random.Range(50, 100), 0, 0);
        ////queue.Enqueue(vc);
        ////if (queue.Count > 5)
        ////    queue.Dequeue();
        ////Vector3 pos = Vector3.zero;
        ////foreach (var item in queue)
        ////    pos += item;
        ////Vector3 averagePos = pos/queue.Count;
        ////pos = Vector3.zero;
        //rImg.transform.localPosition = LinqUtil.QueueAverage(ref queue,vc,500);
        ////rImg.transform.localPosition = Vector3.Lerp(rImg.transform.localPosition, averagePos, Time.deltaTime * 4f);
    }

    private int index;
    private void AA()
    {
        Debug.Log(index.ToString());
        index++;
    }

    [RegisterCommand]
    private void BB()
    {
        Debug.Log(index);
    }

    static bool test_name = true;
    string tips = Extend.GetVarName(test_name,it => test_name);
}

public static class Extend
{
    public static string GetVarName<T>(this T var_name, System.Linq.Expressions.Expression<Func<T, T>> exp)
    {
        return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
    }
}




