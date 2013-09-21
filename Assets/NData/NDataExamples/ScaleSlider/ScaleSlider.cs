using UnityEngine;

public class ScaleSliderContext : EZData.Context
{
	#region Property Scale
	private readonly EZData.Property<int> _privateScaleProperty = new EZData.Property<int>();
	public EZData.Property<int> ScaleProperty { get { return _privateScaleProperty; } }
	public int Scale
	{
	get    { return ScaleProperty.GetValue();    }
	set    { ScaleProperty.SetValue(value); }
	}
	#endregion
}

public class ScaleSlider : MonoBehaviour
{
	public NguiRootContext View;
	public ScaleSliderContext Context;
	
	void Awake()
	{
		Context = new ScaleSliderContext();
		View.SetContext(Context);
	}
}
