using System;
using System.Collections.Generic;
using System.Linq;

namespace RestService.Models
{
    public class CharacterDb
    {
        private readonly List<Character> _characters;

        public CharacterDb()
        {
            _characters = new List<Character>
            {
                new Character {Name = "Alex", Level = 15, Exp = 13400},
                new Character {Name = "Ivan", Level = 10, Exp = 10001}
            };
        }

        public void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        public void RemoveCharacter(string name)
        {
            var characterForDeletion = _characters.FirstOrDefault(character => character.Name == name); 
            
            if (characterForDeletion == null)
                throw new KeyNotFoundException();

            _characters.Remove(characterForDeletion);
        }

        public void RemoveCharacter(Character character)
        {
            if (!_characters.Contains(character))
                throw new ArgumentException();
            
            _characters.Remove(character);

        }

        public Character GetCharacter(string name)
        {
            return _characters.FirstOrDefault(ch => ch.Name == name);
        }

        public List<Character> GetAllCharacters()
        {
            return _characters;
        }
    }
}