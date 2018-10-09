using UICore;
using UnityEngine.UI;

public class FindUI : BaseUI
{
	private RawImage RawImagerImg;
	private Image ImageImg;
	protected override void InitUiOnAwake()
	{
		base.InitUiOnAwake();
		ImageImg = GameTool.GetTheChildComponent<Image>(gameObject,"Image");
		RawImagerImg = GameTool.GetTheChildComponent<RawImage>(gameObject,"RawImage");

	}
	protected override void InitDataOnAwake()
	{

	}
	protected override void RegistBtns()
	{
        base.RegistBtns();
	}
}
