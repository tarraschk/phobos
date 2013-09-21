using UnityEngine;
using InventoryContexts;

namespace InventoryContexts
{
	public enum Slot
	{
		Shoulders = 0,
		Bracers = 1,
		Boots = 2,
	};
	
	public class ClothingItem : EZData.Context
	{
		#region Property Name
		private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
		public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
		public string Name
		{
		get    { return NameProperty.GetValue();    }
		set    { NameProperty.SetValue(value); }
		}
		#endregion
		
		#region Property Slot
		private readonly EZData.Property<int> _privateSlotProperty = new EZData.Property<int>();
		public EZData.Property<int> SlotProperty { get { return _privateSlotProperty; } }
		public Slot Slot
		{
		get    { return (Slot)SlotProperty.GetValue();    }
		set    { SlotProperty.SetValue((int)value); }
		}
		#endregion
		
		#region Property Icon
		private readonly EZData.Property<string> _privateIconProperty = new EZData.Property<string>();
		public EZData.Property<string> IconProperty { get { return _privateIconProperty; } }
		public string Icon
		{
		get    { return IconProperty.GetValue();    }
		set    { IconProperty.SetValue(value); }
		}
		#endregion

		#region Property Model
		private readonly EZData.Property<GameObject> _privateModelProperty = new EZData.Property<GameObject>();
		public EZData.Property<GameObject> ModelProperty { get { return _privateModelProperty; } }
		public GameObject Model
		{
		get    { return ModelProperty.GetValue();    }
		set    { ModelProperty.SetValue(value); }
		}
		#endregion
		
		public event System.Action<ClothingItem> OnToggle;
		public void Toggle()
		{
			if (OnToggle != null)
				OnToggle(this);
		}
	}
	
	public class Character : EZData.Context
	{
		#region Collection Backpack
		private readonly EZData.Collection<ClothingItem> _privateBackpack =
			new EZData.Collection<ClothingItem>(false);
		public EZData.Collection<ClothingItem> Backpack { get { return _privateBackpack; } }
		#endregion
		
		#region Shoulders
		public readonly EZData.VariableContext<ClothingItem> ShouldersEzVariableContext =
			new EZData.VariableContext<ClothingItem>(null);
		public ClothingItem Shoulders
		{
		    get { return ShouldersEzVariableContext.Value; }
		    set { ShouldersEzVariableContext.Value = value; }
		}
		#endregion
		
		#region Bracers
		public readonly EZData.VariableContext<ClothingItem> BracersEzVariableContext =
			new EZData.VariableContext<ClothingItem>(null);
		public ClothingItem Bracers
		{
		    get { return BracersEzVariableContext.Value; }
		    set { BracersEzVariableContext.Value = value; }
		}
		#endregion

		#region Boots
		public readonly EZData.VariableContext<ClothingItem> BootsEzVariableContext =
			new EZData.VariableContext<ClothingItem>(null);
		public ClothingItem Boots
		{
		    get { return BootsEzVariableContext.Value; }
		    set { BootsEzVariableContext.Value = value; }
		}
		#endregion
		
		private EZData.VariableContext<ClothingItem> GetSlot(ClothingItem item)
		{
			switch(item.Slot)
			{
			case Slot.Shoulders: return ShouldersEzVariableContext;
			case Slot.Bracers: return BracersEzVariableContext;
			case Slot.Boots: return BootsEzVariableContext;
			}
			return null;
		}
		
		private void Toggle(ClothingItem item)
		{
			var slot = GetSlot(item); // find a slot corresponding to the item
			if (slot.Value == item) // if item is equipped
			{
				slot.Value = null; // remove it from slot
				Backpack.Add(item); // and put to backpack
			}
			else // otherwise if item is not equipped
			{
				if (slot.Value != null)
					Backpack.Add(slot.Value); // clear the slot if it was used
				
				Backpack.Remove(item); // take item from backpack
				slot.Value = item; // and put to the slot
			}
		}
		
		public void Add(ClothingItem item)
		{
			item.OnToggle += Toggle;
			
			Backpack.Add(item);
		}
		
		public void Remove(ClothingItem item)
		{
			Backpack.Remove(item);
			
			var slot = GetSlot(item);
			if (slot.Value == item)
				slot.Value = null;
			
			item.OnToggle -= Toggle;
		}
	}
}

public class Inventory : MonoBehaviour
{
	public NguiRootContext [] Views;
	public Character Context;
	
	void Awake()
	{
		Context = new Character();
		
		Context.Add(new ClothingItem()
		{
			Name = "Boots of Haste",
			Slot = Slot.Boots,
			Icon = "YellowButton",
		});
		
		Context.Add(new ClothingItem()
		{
			Name = "Bracers of Strength",
			Slot = Slot.Bracers,
			Icon = "RedButton",
		});
		
		Context.Add(new ClothingItem()
		{
			Name = "Bracers of Agility",
			Slot = Slot.Bracers,
			Icon = "GreenButton",
		});
		
		Context.Add(new ClothingItem()
		{
			Name = "Shoulders of Spell Power",
			Slot = Slot.Shoulders,
			Icon = "BlueButton",
		});
		
		foreach (var view in Views)
			view.SetContext(Context);
	}
}
