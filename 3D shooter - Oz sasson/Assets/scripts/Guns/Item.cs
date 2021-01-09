﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public ItemInfo ItemInfo;

	public GameObject itemGameobject;

	public abstract void Use();	
}
