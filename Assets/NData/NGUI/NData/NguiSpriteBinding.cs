using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Sprite Binding")]
public class NguiSpriteBinding : NguiBinding
{
	public string format = "{0}";
	public bool makePixelPerfect = true;
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private UISprite _UiSpriteReceiver;
	
	public override void Awake()
	{
		base.Awake();
		
		_properties.Add(typeof(string), null);
		_properties.Add(typeof(UIAtlas.Sprite), null);

		_UiSpriteReceiver = gameObject.GetComponent<UISprite>();
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange -= OnChange;
				_properties[p.Key] = null;
				break;
			}
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
			
		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiSpriteBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties[typeof(string)] = context.FindProperty<string>(Path, this);
		_properties[typeof(UIAtlas.Sprite)] = context.FindProperty<UIAtlas.Sprite>(Path, this);
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange += OnChange;				
			}
		}
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		var newValue = string.Empty;
		
		if (_properties[typeof(string)] != null)
		{
			newValue = ((EZData.Property<string>)_properties[typeof(string)]).GetValue();
		}
		if (_properties[typeof(UIAtlas.Sprite)] != null)
		{
			var sprite = ((EZData.Property<UIAtlas.Sprite>)_properties[typeof(UIAtlas.Sprite)]).GetValue();
			newValue = sprite != null ? sprite.name : string.Empty;
		}
		
		if (_UiSpriteReceiver != null)
		{
			_UiSpriteReceiver.spriteName = MakeSpriteName(newValue);
			if (makePixelPerfect)
				_UiSpriteReceiver.MakePixelPerfect();
		}
	}
	
	protected virtual string MakeSpriteName(string value)
	{
		return String.Format(NguiUtils.LocalizeFormat(format), value);
	}
}
