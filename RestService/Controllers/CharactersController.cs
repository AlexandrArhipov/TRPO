using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestService.Models;

namespace RestService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private static readonly CharacterDb CharacterDb = new CharacterDb();
        
        [HttpGet]
        public IActionResult GetAll()
        {
            if (CharacterDb.GetAllCharacters() == null || CharacterDb.GetAllCharacters().Count == 0)
                return NotFound();
            
            return new OkObjectResult(CharacterDb.GetAllCharacters());
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            Character character = CharacterDb.GetCharacter(name);

            if (character == null)
                return NotFound();
            
            return new OkObjectResult(character);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Character character)
        {     
            if (character == null)
            {
                return BadRequest();
            }

            CharacterDb.AddCharacter(character);
            return new OkObjectResult(character);
        }

        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] Character character)
        {
            if (character == null || character.Name != name)
            {
                return BadRequest();
            }

            var updatedCharacter = CharacterDb.GetCharacter(name);
            if (updatedCharacter == null)
            {
                return NotFound();
            }

            updatedCharacter.Level = character.Level;
            updatedCharacter.Exp = character.Exp;

            return new OkObjectResult(character);   
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var character = CharacterDb.GetCharacter(name);
            if (character == null)
            {
                return NotFound();
            }

            CharacterDb.RemoveCharacter(character);
                
            return new OkObjectResult(name);
        }
    }
}