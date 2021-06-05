using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewObstacle",menuName ="ScriptableObjects/Obstacle",order =0)]
public class Obstacle : ScriptableObject
{
    public Sprite obstacleSpriteTop;
    public Sprite obstacleSpriteBot;
    public Color obstacleColorTop = Color.white; 
    public Color obstacleColorBot = Color.white;
    public int minScore = 0;

}
