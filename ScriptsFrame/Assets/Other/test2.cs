using UnityEngine;
using UnityEngine.UI;

public class test2 : MonoBehaviour {

    public Transform o1, o4;

    public Texture2D tex;
    public RawImage rImg;
    public PsType type;
    private void Start()
    {
        //o1.LocalMove(o4);
        rImg.texture = TextureUtil.SetTexture(tex, type);
        rImg.SetNativeSize();  
    }

    private void Update()
    {
        //int[] a = new int[] { 1, 2, 3, 4 ,5,6,7,8,9,0};
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.LogError(RandomUtil.NextItem<int>(a));
        //}
    }
}
