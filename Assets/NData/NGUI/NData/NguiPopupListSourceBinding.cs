using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/PopupList Binding")]
public class NguiPopupListSourceBinding : NguiItemsSourceBinding
{
	public string DisplayValuePath;
	
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _displayValuesCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	
	private UIPopupList _uiPopupList = null;
	private GameObject _nativeEventReceiver;
	private string _nativeFunctionName;
	
	public override void Awake()
	{
		base.Awake();
		_uiPopupList = GetComponent<UIPopupList>();
		if (_uiPopupList != null)
		{
			_nativeEventReceiver = _uiPopupList.eventReceiver;
			_nativeFunctionName = _uiPopupList.functionName;
			
			_uiPopupList.eventReceiver = gameObject;
			_uiPopupList.functionName = "OnSelectionChange";
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		if (_uiPopupList == null)
			return;
		OnItemsClear();
		if (_collection == null)
			return;
		for (var i = 0; i < _collection.ItemsCount; ++i)
		{
			_uiPopupList.items.Add(GetItemDisplayValue(i));
		}
		_uiPopupList.selection = GetItemDisplayValue(_collection.SelectedIndex);
	}
		
	protected override void OnItemInsert(int position, EZData.Context item)
	{
		base.OnItemInsert(position, item);
		_uiPopupList.items.Insert(position, GetDisplayValueProperty(item).GetValue());
		_uiPopupList.selection = GetItemDisplayValue(_collection.SelectedIndex);
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_collection == null || _uiPopupList == null)
			return;
		_displayValuesCache.Remove(_collection.GetBaseItem(position));
		base.OnItemRemove(position);
		_uiPopupList.items.RemoveAt(position);
		if (_uiPopupList.items.Count == 0)
			_uiPopupList.selection = string.Empty;
		else
			_uiPopupList.selection = GetItemDisplayValue(_collection.SelectedIndex);
	}
	
	protected override void OnItemsClear()
	{
		_displayValuesCache.Clear();
		_uiPopupList.items.Clear();
		_uiPopupList.selection = string.Empty;
	}
	
	private EZData.Property<string> GetDisplayValueProperty(EZData.Context item)
	{
		if (item == null)
			return null;
		
		EZData.Property<string> property = null;
		if (_displayValuesCache.TryGetValue(item, out property))
			return property;
		property = item.FindProperty<string>(DisplayValuePath, this);
		if (property != null)
			_displayValuesCache.Add(item, property);
		return property;
	}
	
	private string GetItemDisplayValue(int index)
	{
		if (_collection == null)
			return string.Empty;
		var property = GetDisplayValueProperty(_collection.GetBaseItem(index));
		if (property == null)
			return string.Empty;
		return property.GetValue();
	}
	
	public void OnSelectionChange(string selectedItem)
	{
		if (_collection != null && !_isCollectionSelecting)
		{
			_isCollectionSelecting = true;
			for (var i = 0; i < _collection.ItemsCount; ++i)
			{
				if (GetItemDisplayValue(i) == selectedItem)
				{
					_collection.SelectItem(i);
					break;
				}
			}
			_isCollectionSelecting = false;
		}
		
		if (_nativeEventReceiver != null)
		{
			if (_nativeEventReceiver != gameObject || _nativeFunctionName != "OnSelectionChange")
			{
				_nativeEventReceiver.SendMessage(_nativeFunctionName, selectedItem, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	protected override void OnCollectionSelectionChange()
	{
		if (_uiPopupList == null || _collection == null)
			return;
		
		var selectedValue = GetItemDisplayValue(_collection.SelectedIndex);
		_uiPopupList.selection = selectedValue;
	}
}
