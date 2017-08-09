using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * FearMeter keeps track of the player fear statistic and manages its 
 * GUI component by calling ChangeFear(). 
 * 
 * FearMeter modifies the RectTransform attached to the same GameObject.
*/

public class FearMeter : MonoBehaviour {

    public Player player;
    public Animator hurtEffectAnim;
    int hurtHash = Animator.StringToHash("HurtEffect 0");
    const float HURT_THRESHOLD = 3;  // For debugging only

    public float[] maxFears = { 200f, 150f, 100f }; 
    float fear;

    float maxWidth;
    float height;

    private void Start()
    {
        fear = 0;
        height = transform.GetComponent<RectTransform>().rect.height;
        maxWidth = transform.GetComponent<RectTransform>().rect.width;
        UpdateFearMeter();
    }

    /* Increases or decreases the player's fear. Use negative value to decrement. */
    public void ChangeFear(float delta)
    {
        fear += delta;
        //if (delta >= HURT_THRESHOLD || delta <= -HURT_THRESHOLD) print("Player fear: " + fear);
        if (fear >= maxFears[GameController.instance.diffLvl])
        {
            fear = maxFears[GameController.instance.diffLvl];
            player.GameOver();
        }
        else if(fear < 0)
        {
            fear = 0;
        }
        // Determine if hurt animation should be triggered
        if (delta >= Demon.fearIncreaseLvls[GameController.instance.diffLvl]) hurtEffectAnim.SetTrigger("Hurt");
        UpdateFearMeter();
    }

    /* Update the graphical appearence of fear meter to match percentage of max fear. */
    private void UpdateFearMeter()
    {
        float width = (fear / maxFears[GameController.instance.diffLvl]) * maxWidth;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
