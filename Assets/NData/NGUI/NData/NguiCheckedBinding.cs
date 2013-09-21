using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Checked Binding")]
public class NguiCheckedBinding : NguiBooleanBinding
{
	private UICheckbox _checkBox;
	private bool _prevState;
	private bool _ignoreChanges;
	
	public override void Awake()
	{
		base.Awake();
		_checkBox = gameObject.GetComponent<UICheckbox>();
	}
	
	void Update()
	{
		if (_checkBox != null)
		{
			if (_prevState != _checkBox.isChecked)
			{
				_prevState = _checkBox.isChecked;
				_ignoreChanges = true;
				ApplyInputValue(_checkBox.isChecked);
				_ignoreChanges = false;
			}
		}
	}
		
	protected override void ApplyNewValue(bool newValue)
	{
		if (_ignoreChanges)
			return;
		
		if (_checkBox != null)
		{
			_checkBox.isChecked = newValue;
		}
	}
}
