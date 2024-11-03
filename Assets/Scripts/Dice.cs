using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public bool onCollided = false;
	public int number = 0;

	private int lastNumber = 0;

	private void OnCollisionEnter(Collision collision) {
		onCollided = true;
	}

	private void Update() {
		ChangeNumber();
	}

	private void ChangeNumber() {
		if (number != lastNumber) {
			lastNumber = number;
			GameManager.Instance.IncreaseThrows();
			
			
		}
	}

}
