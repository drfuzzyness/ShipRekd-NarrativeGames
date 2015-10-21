using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public enum StoryState { Underwater, Forest, Hell, Surface }
public class StoryController : MonoBehaviour {
	[HeaderAttribute("Balance")]
	public float audioDelay;
	public Color whiteFlash;
	
	[HeaderAttribute("Setup")]
	public StoryState state;
	public PlayerController player;
	public Camera leftCam;
	public Camera rightCam;
	
	[HeaderAttribute("Underwater")]
	public Material underwaterSkybox;
	public Terrain underwaterTerrain;
	public AudioSource underwaterSFX;
	public Color underwaterFogColor;
	public float underwaterFogDensity;
	
	[HeaderAttribute("Forest")]
	public Material forestSkybox;
	public Terrain forestTerrain;
	public AudioSource forestSFX;
	public AudioSource shipSFX;
	public Color forestFogColor;
	public float forestFogDensity;
	
	[HeaderAttribute("Surface")]
	public Material surfaceSkybox;
	public GameObject surfaceTerrain;
	public AudioSource surfaceSFX;
	public Color surfaceFogColor;
	public float surfaceFogDensity;
	public float waitTimeOnSurface;
	public GameObject endUI;
	public Transform surfacePlayerPosition;
	
	public static StoryController instance;
	private Rect fadeRectOverlay;
	private bool isFadeRectInUse;
	
	void Awake() {
		instance = this;
	}
	
	void Start() {
		TriggerUnderwater();
		RenderSettings.fogColor = underwaterFogColor;
		RenderSettings.fogDensity = underwaterFogDensity;
	}
	
	public void TriggerUnderwater() {
		state = StoryState.Underwater;
		
		RenderSettings.skybox = underwaterSkybox;
		RenderSettings.fogColor = underwaterFogColor;
		RenderSettings.fogDensity = underwaterFogDensity;
		
		StartCoroutine( FadeInVolume( underwaterSFX ) );
		StartCoroutine( FadeOutVolume( shipSFX ) );
		StartCoroutine( FadeOutVolume( forestSFX ) );
		
		StartCoroutine( FadeFog( underwaterFogColor, underwaterFogDensity ) );
	}
	
	public void TriggerForest() {
		state = StoryState.Forest;
		StartCoroutine( ForestTransition() );
	}
	
	public void TriggerSurface() {
		state = StoryState.Surface;
		StartCoroutine(SurfaceTransition());
	}
	
	IEnumerator ForestTransition() {
		
		
		
		
		StartCoroutine( FadeOutVolume( underwaterSFX ) );

		
		StartCoroutine( FadeFog( forestFogColor, forestFogDensity ) );
		forestTerrain.gameObject.SetActive(true);
		StartCoroutine( ScreenFade( Color.black ) );
		
		yield return new WaitForSeconds( audioDelay / 2);
		StartCoroutine( FadeInVolume( forestSFX ) );
		StartCoroutine( FadeInVolume( shipSFX ) );
		RenderSettings.skybox = forestSkybox;
		underwaterTerrain.gameObject.SetActive(false);
		forestTerrain.gameObject.SetActive(true);
	}
	
	IEnumerator SurfaceTransition() {
		iTween.CameraFadeAdd( iTween.CameraTexture( Color.black ) );
		iTween.CameraFadeTo( 1f, audioDelay/2 );
		StartCoroutine( FadeOutVolume( underwaterSFX ) );
		
		player.swimming = false;
		player.locked = true;
		
		StartCoroutine( FadeOutVolume(forestSFX) );
		StartCoroutine( FadeOutVolume(shipSFX) );
		
		StartCoroutine( FadeFog( surfaceFogColor, surfaceFogDensity ) );
		player.bubbles.SetActive(false);
		
		yield return new WaitForSeconds( audioDelay/2 );
		surfaceTerrain.SetActive(true);
		
		player.transform.position = surfacePlayerPosition.position;
		RenderSettings.skybox = surfaceSkybox;
		iTween.CameraFadeTo( 0f, audioDelay );
		StartCoroutine( FadeInVolume(surfaceSFX) );
		
		yield return new WaitForSeconds( waitTimeOnSurface + audioDelay );
		
		//  StartCoroutine( FadeOutVolume(surfaceSFX) );
		iTween.CameraFadeTo( 1f, audioDelay/2 );
		yield return new WaitForSeconds( audioDelay / 2);
		// show ending screen
		
	}

	
	IEnumerator FadeOutVolume( AudioSource audio ) {
		float increment = audio.volume / audioDelay;
		for( float countup = 0; countup < audioDelay; countup += Time.deltaTime ) {
			audio.volume -= increment; 
			yield return 0;
		}
		audio.Stop();
	}
	
	IEnumerator FadeInVolume( AudioSource audio ) {
		audio.Play();
		audio.volume = 0;
		float increment = 1f / audioDelay;
		for( float countup = 0; countup < audioDelay; countup += Time.deltaTime ) {
			audio.volume += increment; 
			yield return 0;
		}
	}
	
	IEnumerator ScreenFade( Color targetColor ) {
		iTween.CameraFadeAdd( iTween.CameraTexture( targetColor ) );
		iTween.CameraFadeTo( 1f, audioDelay/2 );
		yield return new WaitForSeconds( audioDelay/2 );
		iTween.CameraFadeTo( 0f, audioDelay/2 );
	}
	
	IEnumerator FadeFog( Color targetColor, float targetDensity ) {
		Color previousColor = RenderSettings.fogColor;
		float previousDensity = RenderSettings.fogDensity;
		for( float timer = 0; timer < audioDelay; timer += Time.deltaTime ) {
			RenderSettings.fogColor = Color.Lerp( previousColor, targetColor, timer / audioDelay );
			RenderSettings.fogDensity = Mathf.Lerp( previousDensity, targetDensity, timer / audioDelay );
			yield return 0;
		}
		RenderSettings.fogColor = targetColor;
		RenderSettings.fogDensity = targetDensity;
	}
	
}
