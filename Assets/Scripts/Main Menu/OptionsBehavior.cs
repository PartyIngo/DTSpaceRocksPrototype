using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsBehavior : MonoBehaviour
{
    [SerializeField]
    private Slider masterVolumeSlider;

    //[SerializeField]
    //private Text masterVolumeText;

    [SerializeField]
    private InputField masterVolumeInput;

    [SerializeField]
    private Slider musicVolumeSlider;

    //[SerializeField]
    //private Text musicVolumeText;

    [SerializeField]
    private InputField musicVolumeInput;

    [SerializeField]
    private Slider sfxVolumeSlider;

    //[SerializeField]
    //private Text sfxVolumeText;

    [SerializeField]
    private InputField sfxVolumeInput;

    public static float masterVolume;
    public static float musicVolume;
    public static float sfxVolume;

    float masterVolumeRaw = 0;
    float musicVolumeRaw = 0;
    float sfxVolumeRaw = 0;

    void Start()
    {
        masterVolumeInput.text = "0";
        musicVolumeInput.text = "0";
        sfxVolumeInput.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Master Volume Raw: " + masterVolumeRaw);
        //Debug.Log("Master Volume: " + masterVolume);
        //Debug.Log("Music Volume Raw: " + musicVolumeRaw);
        //Debug.Log("Music Volume: " + musicVolume);
        //Debug.Log("SFX Volume Raw: " + sfxVolumeRaw);
        //Debug.Log("SFX Volume: " + sfxVolume);
    }



    /**
     *  Stores the values of the settings in the respective variables
     */
    void storeValues()
    {
        masterVolume = masterVolumeRaw;
        musicVolume = musicVolumeRaw / 100 * masterVolume;  //musicVolumeRaw;//((musicVolumeRaw  / 100) * masterVolume); //Master Volume and music volume need to be in a relation, therefore this formula is needed
        sfxVolume = sfxVolumeRaw / 100 * masterVolume;//((sfxVolumeRaw    / 100) * masterVolume); //Master Volume and SFX volume need to be in a relation, therefore this formula is needed
    }


    /**
     * Changes the value of master volume based on the slider's value when it's changed
     */
    public void setMasterVolumeSlider()
    {
        masterVolumeRaw = (float)masterVolumeSlider.value;
        masterVolumeInput.text = masterVolumeRaw.ToString();

        //Stores changes in variables for the game
        storeValues();
    }

    /**
     *  Changes the value of master volume based on the input field's value when it's changed
     */
    public void setMasterVolumeInput()
    {
        //Convert the input field value (text) to an integer
        masterVolumeRaw = float.Parse(masterVolumeInput.text);

        //Values only between 0-100
        if (masterVolumeRaw > 100)
        {
            masterVolumeRaw = 100;
        }
        if (masterVolumeRaw < 0)
        {
            masterVolumeRaw = 0;
        }
        //Needed if the value is out of boundaries. New, valid value is now shown
        masterVolumeInput.text = masterVolumeRaw.ToString();
        masterVolumeSlider.value = masterVolumeRaw;

        //Stores changes in variables for the game
        storeValues();
    }

    /**
     *  Changes the value of music volume based on the slider's value when it's changed
     */
    public void setMusicVolumeSlider()
    {
        musicVolumeRaw = (float)musicVolumeSlider.value;
        musicVolumeInput.text = musicVolumeRaw.ToString();

        //Stores changes in variables for the game
        storeValues();
    }

    /**
     *  Changes the value of music volume based on the input field's value when it's changed
     */
    public void setMusicVolumeInput()
    {
        //Convert the input field value (text) to an integer
        musicVolumeRaw = float.Parse(musicVolumeInput.text);

        //Values only between 0-100
        if (musicVolumeRaw > 100)
        {
            musicVolumeRaw = 100;
        }
        if (musicVolumeRaw < 0)
        {
            musicVolumeRaw = 0;
        }
        //Needed if the value is out of boundaries. New, valid value is now shown
        musicVolumeInput.text = musicVolumeRaw.ToString();
        musicVolumeSlider.value = musicVolumeRaw;

        //Stores changes in variables for the game
        storeValues();
    }


    /**
     *  Changes the value of SFX volume based on the slider's value when it's changed
     */
    public void setSFXVolumeSlider()
    {
        sfxVolumeRaw = (float)sfxVolumeSlider.value;
        sfxVolumeInput.text = sfxVolumeRaw.ToString();

        //Stores changes in variables for the game
        storeValues();
    }

    /**
     * Changes the value of SFX volume based on the input field's value, when it's changed
     */
    public void setSFXVolumeInput()
    {
        //Convert the input field value (text) to an integer
        sfxVolumeRaw = float.Parse(sfxVolumeInput.text);

        //Values only between 0-100
        if (sfxVolumeRaw > 100)
        {
            sfxVolumeRaw = 100;
        }
        if (sfxVolumeRaw < 0)
        {
            sfxVolumeRaw = 0;
        }
        //Needed if the value is out of boundaries. New, valid value is now shown
        sfxVolumeInput.text = sfxVolumeRaw.ToString();
        sfxVolumeSlider.value = sfxVolumeRaw;

        //Stores changes in variables for the game
        storeValues();
    }

}
