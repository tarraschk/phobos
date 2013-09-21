using UnityEngine;
 
public class TestUi : EZData.Context
{
	#region Property Volume
	private readonly EZData.Property<float> _privateVolumeProperty = new EZData.Property<float>();
	public EZData.Property<float> VolumeProperty { get { return _privateVolumeProperty; } }
	public float Volume
	{
	get    { return VolumeProperty.GetValue();    }
	set    { VolumeProperty.SetValue(value); }
	}
	#endregion
}
  
public class ViewModel : MonoBehaviour
{
 public NguiRootContext View;
 public TestUi Context;
  
 void Awake()
 {
  Context = new TestUi();
  View.SetContext(Context);
 }
}


