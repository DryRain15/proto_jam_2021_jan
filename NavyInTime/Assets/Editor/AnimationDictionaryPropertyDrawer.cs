using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationDictionary))]
[CustomPropertyDrawer(typeof(AutoTransitionDictionary))]
public class AnimationDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{
}
