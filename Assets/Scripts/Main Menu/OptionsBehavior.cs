using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsBehavior : MonoBehaviour
{
    [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField]
    private Text masterVolumeText;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Text musicVolumeText;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Text sfxVolumeText;

    public static int masterVolume;
    public static int musicVolume;
    public static int sfxVolume;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //represents values in UI
        showValues();

        //Stores changes in variables for the game
        storeValues();

        //handles the slider step size, when dragged over longer time
        increaseStepSize();
    }



    void increaseStepSize()
    {
        //IF Value change registered
            //set timer
            //IF timer > X Seconds
                //increase step size
    }






    /**
     * represents the current values of the sliders in the UI
     */
    void showValues()
    {
        masterVolumeText.text = masterVolumeSlider.value.ToString();
        musicVolumeText.text = musicVolumeSlider.value.ToString();
        sfxVolumeText.text = sfxVolumeSlider.value.ToString();
    }


    /**
     * Stores the values of the settings in the respective variables
     */
    void storeValues()
    {
        masterVolume = (int)masterVolumeSlider.value;
        musicVolume = (((int)musicVolumeSlider.value / 100) * masterVolume) / 100; //Master Volume and music volume need to be in a relation, therefore this formula is needed
        sfxVolume = (((int)sfxVolumeSlider.value / 100) * masterVolume) / 100; //Master Volume and SFX volume need to be in a relation, therefore this formula is needed
    }


}
