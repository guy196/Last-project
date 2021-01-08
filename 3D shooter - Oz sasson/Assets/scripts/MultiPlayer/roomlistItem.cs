using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class roomlistItem : MonoBehaviour
{
	[SerializeField] TMP_Text text;

	public RoomInfo info;
	public void	Setup(RoomInfo _info)
	{
		info = _info;
		text.text = info.Name;
	}

	public void onClick()
	{
		Luncher.Instance.JoinRoom(info);
	}
}
