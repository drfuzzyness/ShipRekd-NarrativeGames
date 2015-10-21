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
	
	[HeaderAttribute("Hell")]
	public Material hellSkybox;
	public Terrain hellTerrain;
	public AudioSource hellSFX;
	public Color hellFogColor;
	
	[HeaderAttribute("Surface")]
	public Material surfaceSkybox;
	public AudioSource surfaceSFX;
	public Color surfaceFogColor;
	
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
		
		RenderSettings.skybox = forestSkybox;
		
		StartCoroutine( FadeInVolume( forestSFX ) );
		StartCoroutine( FadeOutVolume( underwaterSFX ) );
		StartCoroutine( FadeInVolume( shipSFX ) );
		
		StartCoroutine( FadeFog( forestFogColor, forestFogDensity ) );
		underwaterTerrain.gameObject.SetActive(false);
		forestTerrain.gameObject.SetActive(true);
		StartCoroutine( ScreenFade( Color.black ) );
	}
	
	public void TriggerHell() {
		state = StoryState.Hell;
		StartCoroutine( FadeOutVolume( shipSFX ) );
		
		StartCoroutine( FadeFog( hellFogColor, forestFogDensity ) );
	}
	
	public void TriggerSurface() {
		state = StoryState.Surface;
		
		StartCoroutine( FadeFog( surfaceFogColor, forestFogDensity ) );
		
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
	
	
	void OnGUI() {
		
	}
	
}
