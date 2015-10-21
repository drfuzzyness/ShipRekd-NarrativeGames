using UnityEngine;
using System.Collections;

public class StoryTrigger : MonoBehaviour {
	
	public StoryState state;

	void OnTriggerEnter() {
		switch (state) {
			case StoryState.Forest:
				StoryController.instance.TriggerForest();
				break;
			case StoryState.Hell:
				StoryController.instance.TriggerHell();
				break;
			case StoryState.Underwater:
				StoryController.instance.TriggerUnderwater();
				break;
			case StoryState.Surface:
				StoryController.instance.TriggerSurface();
				break;
		}
	}
}
