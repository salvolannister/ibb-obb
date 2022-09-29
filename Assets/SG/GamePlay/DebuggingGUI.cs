using Assets.SG.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggingGUI : MonoBehaviour
{
    public PlayerGravityHandler pg;
    public Text gravityIsReversedTxt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string reversed = pg.IsReversed == ((int)PlayerGravityHandler.gravityValues.normal) ? "normal" : "reversed";
        gravityIsReversedTxt.text = $"Gravity for {pg.gameObject.name} is  {reversed} ";

    }
}
