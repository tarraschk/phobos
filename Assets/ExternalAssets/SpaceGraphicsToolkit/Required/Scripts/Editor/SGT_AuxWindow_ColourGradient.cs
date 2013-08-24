using UnityEngine;
using UnityEditor;

public class SGT_AuxWindow_ColourGradient : SGT_AuxWindow<SGT_ColourGradient, SGT_AuxWindow_ColourGradient>
{
	private SGT_ColourGradient.ColourNode selectedColourNode;
	private SGT_ColourGradient.AlphaNode  selectedAlphaNode;
	private bool       draggingColourNode;
	private bool       removeColourNode;
	private bool       draggingAlphaNode;
	private bool       removeAlphaNode;
	private Color clr;
	public override void OnSetupAuxWindow()
	{
		title = "Colour Gradient";
		
		if (Target.HasAlpha == true)
		{
			minSize = new Vector2(300.0f, 155.0f);
			maxSize = new Vector2(600.0f, 155.0f);
		}
		else
		{
			minSize = new Vector2(300.0f, 120.0f);
			maxSize = new Vector2(600.0f, 120.0f);
		}
	}
	
	public override void OnInspector()
	{
		var dragRect      = SGT_RectHelper.RemovePx(SGT_EditorGUI.Reserve(0.0f), 20.0f, 20.0f, 0.0f, 0.0f);
		var mousePosition = SGT_EditorGUI.GetHorizontalSliderAcross(dragRect, Event.current.mousePosition.x);
		
		// Draw alpha stuff
		if (Target.HasAlpha == true)
		{
			// Space for slider
			SGT_EditorGUI.Reserve(16.0f);
			
			var alphaNodes = Target.AlphaNodes;
			var alphaRect  = SGT_EditorGUI.Reserve(16.0f);
			var alphaRect2 = SGT_RectHelper.ExpandPx(alphaRect, 20.0f);
			
			alphaRect = SGT_RectHelper.RemovePx(alphaRect, 20.0f, 20.0f, 0.0f, 0.0f);
			
			SGT_EditorGUI.DrawHorizontalSlider(alphaRect);
			
			// Draw alpha nodes
			foreach (var node in alphaNodes)
			{
				bool selected = node == selectedAlphaNode;
				Rect thumb;
				
				if (selected == false || removeAlphaNode == false)
				{
					SGT_EditorGUI.BeginFrozen(selected);
					{
						thumb = SGT_EditorGUI.DrawHorizontalSliderThumb(alphaRect, node.Position);
					}
					SGT_EditorGUI.EndFrozen();
					
					if (selected == true)
					{
						var sliderRect = new Rect(0.0f, 0.0f, 40.0f, 16.0f);
						sliderRect.center = new Vector2(thumb.x, thumb.yMin - 13.0f);
						
						var oldAlpha = node.Alpha;
						
						node.SetAlpha(Target, GUI.HorizontalSlider(sliderRect, node.Alpha, 0.0f, 1.0f));
						
						SGT_EditorGUI.MarkModified(Mathf.Approximately(oldAlpha, node.Alpha) == false, IsField);
					}
				}
			}
			
			// Handle event specific stuff
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if (alphaRect.Contains(Event.current.mousePosition) == true)
				{
					selectedAlphaNode = Target.FindClosestAlphaNode(mousePosition);
					removeAlphaNode   = false;
					
					if (selectedAlphaNode != null)
					{
						var distance   = Mathf.Abs(selectedAlphaNode.Position - mousePosition);
						var distancePx = distance * alphaRect.width;
						
						if (distancePx > 20.0f)
						{
							selectedAlphaNode = Target.AddAlphaNode(1.0f, mousePosition);
							
							SGT_EditorGUI.MarkModified(true, IsField);
						}
					}
					else
					{
						selectedAlphaNode = Target.AddAlphaNode(1.0f, mousePosition);
						
						SGT_EditorGUI.MarkModified(true, IsField);
					}
					
					if (selectedAlphaNode.Locked == true)
					{
						draggingAlphaNode = false;
						selectedAlphaNode = null;
					}
					else
					{
						draggingAlphaNode = true;
					}
					
					Repaint();
				}
				break;
				
			case EventType.MouseUp:
				if (selectedAlphaNode != null && draggingAlphaNode == true && removeAlphaNode == true)
				{
					Target.RemoveAlphaNode(selectedAlphaNode);
					
					SGT_EditorGUI.MarkModified(true, IsField);
				}
				
				draggingAlphaNode = false;
				removeAlphaNode   = false;
				break;
				
			case EventType.MouseDrag:
				if (selectedAlphaNode != null && draggingAlphaNode == true)
				{
					var oldPosition = selectedAlphaNode.Position;
					
					selectedAlphaNode.SetPosition(Target, mousePosition);
					
					SGT_EditorGUI.MarkModified(Mathf.Approximately(oldPosition, selectedAlphaNode.Position) == false, IsField);
					
					// Dragged out?
					removeAlphaNode = alphaRect2.Contains(Event.current.mousePosition) == false;
					
					Repaint();
				}
				break;
			}
		}
		
		// Draw gradient
		{
			var gradientRect = SGT_EditorGUI.Reserve(50.0f);
			
			gradientRect = SGT_RectHelper.RemovePx(gradientRect, 20.0f, 20.0f, 0.0f, 0.0f);
			
			var colours = Target.CalculateColours(0.0f, 1.0f, 256);
			var texture = SGT_ColourGradient.AllocateTexture(256);
			
			for (var x = 0; x < 256; x++)
			{
				texture.SetPixel(x, 0, colours[x]);
			}
			
			texture.Apply();
			
			SGT_EditorGUI.DrawTiledTexture(gradientRect, SGT_Helper.CheckerTexture);
			GUI.DrawTexture(gradientRect, texture);
			
			SGT_Helper.DestroyObject(texture);
		}
		
		// Draw colour stuff
		{
			var colourNodes = Target.ColourNodes;
			var colourRect  = SGT_EditorGUI.Reserve(16.0f);
			var colourRect2 = SGT_RectHelper.ExpandPx(colourRect, 20.0f);
			
			colourRect = SGT_RectHelper.RemovePx(colourRect, 20.0f, 20.0f, 0.0f, 0.0f);
			
			SGT_EditorGUI.DrawHorizontalSlider(colourRect);
			
			// Draw colour keys
			foreach (var node in colourNodes)
			{
				bool selected = node == selectedColourNode;
				Rect thumb;
				
				if (selected == false || removeColourNode == false)
				{
					SGT_EditorGUI.BeginFrozen(selected);
					{
						thumb = SGT_EditorGUI.DrawHorizontalSliderThumb(colourRect, node.Position);
					}
					SGT_EditorGUI.EndFrozen();
					
					if (selected == true)
					{
						var pickerRect = new Rect(0.0f, 0.0f, 40.0f, 20.0f);
						pickerRect.center = new Vector2(thumb.x, thumb.yMax + 15.0f);
						
						var oldColour = node.Colour;
						
						node.SetColour(Target, SGT_EditorGUI.DrawColourPicker(pickerRect, node.Colour));
						
						SGT_EditorGUI.MarkModified(SGT_Helper.Approximately(oldColour, node.Colour) == false, IsField);
					}
				}
			}
			
			// Handle clicking and dragging for the colour nodes
			switch (Event.current.type)
			{
			case EventType.MouseDown:
				if (colourRect.Contains(Event.current.mousePosition) == true)
				{
					selectedColourNode = Target.FindClosestColourNode(mousePosition);
					
					if (selectedColourNode != null)
					{
						var distance   = Mathf.Abs(selectedColourNode.Position - mousePosition);
						var distancePx = distance * colourRect.width;
						
						if (distancePx > 20.0f)
						{
							selectedColourNode = Target.AddColourNode(Color.white, mousePosition);
							
							SGT_EditorGUI.MarkModified(true, IsField);
						}
					}
					else
					{
						selectedColourNode = Target.AddColourNode(Color.white, mousePosition);
						
						SGT_EditorGUI.MarkModified(true, IsField);
					}
					
					if (selectedColourNode.Locked == true)
					{
						draggingColourNode = false;
					}
					else
					{
						draggingColourNode = true;
					}
					
					Repaint();
				}
				break;
				
			case EventType.MouseUp:
				if (selectedColourNode != null && draggingColourNode == true && removeColourNode == true)
				{
					Target.RemoveColourNode(selectedColourNode);
					
					SGT_EditorGUI.MarkModified(true, IsField);
				}
				
				draggingColourNode = false;
				removeColourNode   = false;
				
				Repaint();
				break;
				
			case EventType.MouseDrag:
				if (selectedColourNode != null && draggingColourNode == true)
				{
					var oldPosition = selectedColourNode.Position;
					
					selectedColourNode.SetPosition(Target, mousePosition);
					
					SGT_EditorGUI.MarkModified(Mathf.Approximately(oldPosition, selectedColourNode.Position) == false, IsField);
					
					// Dragged out?
					removeColourNode = colourRect2.Contains(Event.current.mousePosition) == false;
					
					Repaint();
				}
				break;
			}
		}
	}
}