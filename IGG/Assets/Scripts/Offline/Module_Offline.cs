using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Module_Offline : MonoBehaviour
{
	// SERIALIZE FIELD VARIABLES
	[SerializeField] GameObject m_gates;
	[SerializeField] GameObject topGate;
	[SerializeField] GameObject bottomGate;
	[SerializeField] GameObject leftGate;
	[SerializeField] GameObject rightGate;

	// PROPERTIES
	public GameObject gates
	{
		get
		{
			return m_gates;
		}
		private set
		{
			m_gates = value;
		}
	}

	public bool topGateEnabled
	{
		get
		{
			return topGate.activeSelf;
		}
		set
		{
			topGate.SetActive (value);
		}
	}

	public bool bottomGateEnabled
	{
		get
		{
			return bottomGate.activeSelf;
		}
		set
		{
			bottomGate.SetActive (value);
		}
	}

	public bool leftGateEnabled
	{
		get
		{
			return leftGate.activeSelf;
		}
		set
		{
			leftGate.SetActive (value);
		}
	}

	public bool rightGateEnabled
	{
		get
		{
			return rightGate.activeSelf;
		}
		set
		{
			rightGate.SetActive (value);
		}
	}
}