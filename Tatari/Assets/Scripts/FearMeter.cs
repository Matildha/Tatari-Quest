using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FearMeter : MonoBehaviour {

    public Player player;
    public Animator hurtEffectAnim;
    int hurtHash = Animator.StringToHash("HurtEffect 0");
    //const float HURT_THRESHOLD = 5;

    public const float MAX_FEAR = 100f; 
    float fear;

    float maxWidth;
    float height;

    private void Start()
    {
        fear = 85;
        height = transform.GetComponent<RectTransform>().rect.height;
        maxWidth = transform.GetComponent<RectTransform>().rect.width;
        UpdateFearMeter();
    }

    /* Increases or decreases the player's fear. Use negative value to decrement. */
    public void ChangeFear(float delta)
    {
        fear += delta;
        if (delta >= 3 || delta <= -3) print("Player fear: " + fear);
        if (fear >= MAX_FEAR)
        {
            //print("GAMEEEE OOOOOVEEEERRRRR!!!! :O");
            fear = MAX_FEAR;
            player.GameOver();
        }
        else if(fear < 0)
        {
            fear = 0;
        }
        if (delta >= Demon.fearIncreaseLvls[GameController.instance.diffLvl]) hurtEffectAnim.SetTrigger("Hurt");
        UpdateFearMeter();
    }

    private void UpdateFearMeter()
    {
        float width = (fear / MAX_FEAR) * maxWidth;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
