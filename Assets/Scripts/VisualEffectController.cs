using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualEffectController : MonoBehaviour
{
    [SerializeField] private Volume[] playerVolumes; // Array to store all player volumes (size 4 for max players)
    private LensDistortion[] lensDistortions;
    private ColorAdjustments[] colorAdjustments;
    private Vignette[] vignettes;
    private ColorCurves[] colorCurves;

    private int activePlayerCount; // Number of currently active players
    
    private void Awake()
    {
        // Dynamically determine the number of active players
        activePlayerCount = Mathf.Clamp(playerVolumes.Length, 1, 4);

        // Initialize arrays for specific overrides
        lensDistortions = new LensDistortion[activePlayerCount];
        colorAdjustments = new ColorAdjustments[activePlayerCount];
        vignettes = new Vignette[activePlayerCount];
        colorCurves = new ColorCurves[activePlayerCount];

        // Cache the overrides for each active player's Volume
        for (int i = 0; i < activePlayerCount; i++)
        {
            VolumeProfile profile = playerVolumes[i].profile;

            if (profile.TryGet(out LensDistortion lensDist)) lensDistortions[i] = lensDist;
            if (profile.TryGet(out ColorAdjustments colorAdj)) colorAdjustments[i] = colorAdj;
            if (profile.TryGet(out Vignette vignette)) vignettes[i] = vignette;
            if (profile.TryGet(out ColorCurves curves)) colorCurves[i] = curves;
        }

    }

    private void Start()
    {
        //_fullScreenDamage.SetActive(false);
    }

    // Call this when the player is in "ball" state
    public void ActivateBallState(int playerIndex)
    {
        //if (!IsValidPlayer(playerIndex)) return;

        // Enable Lens Distortion and Color Adjustment for the selected player
        if (lensDistortions[playerIndex] != null) lensDistortions[playerIndex].active = true;
        if (colorAdjustments[playerIndex] != null) colorAdjustments[playerIndex].active = true;

        // Disable other effects
        if (vignettes[playerIndex] != null) vignettes[playerIndex].active = false;
        if (colorCurves[playerIndex] != null) colorCurves[playerIndex].active = false;
    }

    //Disable ball state
    public void DeactivateBallState(int playerIndex)
    {
        //if (!IsValidPlayer(playerIndex)) return;

        // Disable Lens Distortion and Color Adjustment for the selected player
        if (lensDistortions[playerIndex] != null) lensDistortions[playerIndex].active = false;
        if (colorAdjustments[playerIndex] != null) colorAdjustments[playerIndex].active = false;
    }

    // Call this when the player is "dead"
    public void ActivateDeadState(int playerIndex)
    {
        //if (!IsValidPlayer(playerIndex)) return;

        // Enable Vignette and Color Curves for the selected player
        if (vignettes[playerIndex] != null) vignettes[playerIndex].active = true;
        if (colorCurves[playerIndex] != null) colorCurves[playerIndex].active = true;

        // Disable other effects
        if (lensDistortions[playerIndex] != null) lensDistortions[playerIndex].active = false;
        if (colorAdjustments[playerIndex] != null) colorAdjustments[playerIndex].active = false;
    }

    /*// Utility method to validate the player index
    private bool IsValidPlayer(int playerIndex)
    {
        return playerIndex >= 0 && playerIndex < activePlayerCount;
    }
    */
}