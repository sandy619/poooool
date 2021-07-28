using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pool_Player {
	private IList<Int32> ballsCollected = new List<Int32>();

	public Pool_Player(string name) {
		Name = name;
	}

	public string Name {
		get;
		private set;
	}

	public int Points {
		get { return ballsCollected.Count; }
	}

	public void Collect(int ballNumber) {
		Debug.Log(Name + " collected ball " + ballNumber);
		ballsCollected.Add(ballNumber);
	}
}
