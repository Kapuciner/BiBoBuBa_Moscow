using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Vegetable", menuName = "ScriptableObjects/Vegetables", order = 1)]
public class VegetableType : ScriptableObject
{
    public AudioClip DeathSound;
    public AudioClip ShootSound;
    public AudioClip AbilitySound;
    public enum SkillSet
    {
        tomato, watermelon, garlic
    }

    public SkillSet Skills;

    public Sprite Portrait;
    public Sprite Sprite;
    
    public float mass;
    public float speed;
    public int health;

    public float abilityCooldown;
    public float attackCooldown;

    public RuntimeAnimatorController Animator;
}
