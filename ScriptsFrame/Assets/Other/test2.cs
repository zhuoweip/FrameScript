using UnityEditor;
using UnityEngine;

public class test2 : MonoBehaviour {

    private void Update()
    {
        int[] a = new int[] { 1, 2, 3, 4 ,5,6,7,8,9,0};
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.LogError(RandomUtil.NextItem<int>(a));
        }
    }
}
