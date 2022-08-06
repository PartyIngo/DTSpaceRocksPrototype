using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopupBehavior : MonoBehaviour
{

    //[Tooltip("The duration until this Object will be destroyed.")] [SerializeField] private float lifeTime;
    [Tooltip("The value, how much this Object should change in scale per Frame.")] [SerializeField] [Range(0.0001f, 0.001f)] private float scaleFactor;

    [Tooltip("The value of the text, that is added to the player's score.")] private int scoreValue;

    private TextMeshPro textMesh;
    private Color textColor;
    [Tooltip("The value, how much of opacity should decrease per Frame.")][SerializeField] private float fade;

    private Vector3 scale;
    private Vector3 newScale;


    // Initialize the Score Popup by setting a value for text and the lifetime of the game object.
    void Start()
    {
        //Destroy(gameObject, lifeTime);
        newScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        textMesh = transform.GetComponent<TextMeshPro>();
        textMesh.SetText("+" + scoreValue.ToString());

        textColor = textMesh.color;

    }

    // While alive, change the scale and the opacity continuously
    void Update()
    {
        textColor.a -= fade;
        textMesh.color = textColor;

        scale = transform.localScale;
        transform.localScale = scale + newScale;

        //DEstroy the GameObject, when it is no longer visible
        if (textMesh.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }


    //receives the score value, that will be shown as a popup
    public void setScoreValue(int newValue)
    {
        scoreValue = newValue;
    }
}
