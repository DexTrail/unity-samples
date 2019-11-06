using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public static int pinCount;
	public Text text;

	void Start () {
		pinCount = 0;
	}

	void Update () {
		text.text = pinCount.ToString ();
	}
}
