using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A ScriptableObject that stores a list of references to Character
// objects.
[CreateAssetMenu(fileName = "CharacterList", menuName = "Visual Novel/Character List", order = 0)]
public class CharacterList : ScriptableObject {
    public List<Character> characters = new List<Character>();

    // Given a character name, return the prefab for the character that has
    // that name
    public Character FindCharacterPrefab(string name) {
        foreach (var character in characters) {
            if (character.name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase)) {
                return character;
            }
        }

        return null;
    }
}