using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewObstacle",menuName ="ScriptableObjects",order =0)]
public class Obstacle : ScriptableObject
{
    public Sprite sprite;
    public Color color = Color.white;
    public int minScore = 0;



}
