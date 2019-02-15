﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bug : MonoBehaviour {

	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	void onMouseMovement()
	{
		Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
	}
	void OnMouseExit()
	{
		Cursor.SetCursor (null, Vector2.zero, cursorMode);
	}
}