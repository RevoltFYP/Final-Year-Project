using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowTimeScript : MonoBehaviour {

    [Header("Slow Effect")]
    public Text modeText;
    public float slowAmount;
    public float transitionTime;
    public KeyCode slowDownKey;

    [Header("Bar")]
    public float slowTotal;
    public float cost;
    public float recoverAmount;

    private float innerSlow;
    private bool disabled = false;


    [Header("VFX")]
    public ParticleSystem startEffect;
    public ParticleSystem endEffect;

    private bool buffering = false;
    private float bufferTime;
    private float internalTime;

    private bool slow = false;

    private float velocity;

    [Header("UI")]
    public GameObject slowUI;
    public Image circularFillImage;
    public Text percentageText;

    // Use this for initialization
    void Awake ()
    {
        innerSlow = slowTotal;

        slowUI.gameObject.SetActive(false);
	}

    private void OnEnable()
    {
        slowUI.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        //slowSlider.value = innerSlow;
        SlowUI();
        OverUse();

        //Debug.Log("Buffering: " + buffering);
        //Debug.Log("Buffer Time: " + bufferTime);

        // Checks when the button is pressed
        if (Input.GetKeyDown(slowDownKey))
        {
            // If VFX is buffering  cannot slow time
            if (!buffering)
            {
                //Debug.Log("Buffer False");
                Toggle(); // buffering is true

                slow = !slow;

                if (!disabled)
                {
                    PlayVFX(slow ? startEffect : endEffect);
                }

                bufferTime = slow ? startEffect.main.duration * slowAmount : endEffect.main.duration * slowAmount;

                Invoke("Toggle", bufferTime); // set buffering back to false
            }
        }

        if (slow)
        {
            if (innerSlow > 0 && !disabled)
            {
                //Debug.Log("Slow On");
                SlowTime();
            }
            else
            {
                //Debug.Log("Slow Off");
                NormalTime();
            }
        }
        else
        {
            //Debug.Log("Slow False");
            if (innerSlow < slowTotal)
            {
                //Debug.Log("Adding");
                NormalTime();
            }
        }

        //Debug.Log("Fixed Delta Time: " + Time.fixedDeltaTime);
        //Debug.Log("Time Scale: " + Time.timeScale);
    }

    // Slows time by slow amount for slow duration of seconds //
    private void SlowTime()
    {
        TransitionSlow();
        innerSlow -= cost;

        modeText.text = "Mode : SlowTime";

        // Slows down physics calculation to make it match time slow
        //Time.timeScale = slowAmount;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void NormalTime()
    {
        TransitionNormal();
        innerSlow += recoverAmount;

        //Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f;

        modeText.text = "Mode : NormalTime";

    }

    // Checks if the bar reaches 0 and performs accordingly
    private void OverUse()
    {
        // Checks if slow reaches 0
        if (innerSlow <= 0)
        {
            //Debug.Log("OverUsed");
            disabled = true;
            slow = false;
            innerSlow = 0;

            PlayVFX(endEffect);
        }
        else
        {
            // Reset innerSlow if innerSlow is over total 
            if (innerSlow >= slowTotal)
            {
                disabled = false;
                innerSlow = slowTotal;
            }
        }
    }

    private void Toggle()
    {
        Debug.Log("Toggle Called");
        buffering = !buffering;
    }

    // Transitions Time scale and delta time to target values
    private void TransitionNormal()
    {
        if (Time.timeScale < 1.0f)
        {
            Time.timeScale += (1f / transitionTime) * Time.unscaledDeltaTime;

            // Ensures that the value will reach back to 1
            if(Time.timeScale >= 0.99f)
            {
                Time.timeScale = 1.0f;
            }
        }

        if (Time.fixedDeltaTime < 0.02f)
        {
            Time.fixedDeltaTime += (0.02f / transitionTime) * Time.unscaledDeltaTime;

            // Ensures that the value will reach back to 0.02f
            if (Time.fixedDeltaTime >= 0.19f)
            {
                Time.fixedDeltaTime = 0.02f;
            }
        }
    }
    private void TransitionSlow()
    {
        if (Time.timeScale > slowAmount)
        {
            Time.timeScale -= (1f / transitionTime) * Time.unscaledDeltaTime;

            if(Time.timeScale <= slowAmount)
            {
                Time.timeScale = slowAmount;
            }
        }

        if (Time.fixedDeltaTime > Time.timeScale * 0.02f)
        {
            Time.fixedDeltaTime -= (0.02f / transitionTime) * Time.unscaledDeltaTime;

            if (Time.fixedDeltaTime <= Time.timeScale * 0.02f)
            {
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }
    }

    // Plays Chosen VFX
    private void PlayVFX(ParticleSystem playFX)
    {
        if (!playFX.isPlaying)
        {
            //Debug.Log("Particle Playing");
            playFX.Play();
        }
    }

    private void SlowUI()
    {
        percentageText.text = ((int)((innerSlow / slowTotal) * 100)).ToString() + "%";
        circularFillImage.fillAmount = ((innerSlow / slowTotal));
    }
}
