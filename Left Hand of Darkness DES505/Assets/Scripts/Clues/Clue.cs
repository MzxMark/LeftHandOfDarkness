using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clue", menuName = "ClueSO", order = 0)]
public class Clue : ScriptableObject {
    [Tooltip("Title of the clue.  This should only be 2-5 words.")]
    /// <summary>
    /// Title of the clue.  This should only be 2-5 words.
    /// </summary>
    public string title;
    [Tooltip("The set this clue is a part of.")]
    /// <summary>
    /// The set this clue is a part of.
    /// </summary>
    public int correctSet;
    [Tooltip("The set this clue is currently attributed to.")]
    /// <summary>
    /// The set this clue is currently attributed to.  This should be 0 if the clue is unassigned.
    /// </summary>
    public int currentSet;
    [Tooltip("Does the player currently possess this clue?")]
    /// <summary>
    /// Does the player currently possess this clue?
    /// </summary>
    public bool playerOwned;

    [Space(10)]
    [Tooltip("The picture attributed to this clue.")]
    /// <summary>
    /// The picture attributed to this clue.
    /// </summary>
    public Sprite sprite;

    [Space(10)]
    [Tooltip("The short (1-2 sentence) description of this clue.")]
    /// <summary>
    /// The short (1-2 sentence) description of this clue.
    /// </summary>
    public string shortDescription;
    [Tooltip("The longer description of this clue.")]
    /// <summary>
    /// The longer description of this clue.
    /// </summary>
    public string longDescription;
}
