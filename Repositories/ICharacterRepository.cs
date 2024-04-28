﻿using FantasyChas_Backend.Data;
using FantasyChas_Backend.Models;
using FantasyChas_Backend.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FantasyChas_Backend.Repositories
{
    public interface ICharacterRepository
    {
        public Task<List<CharacterViewModel>> GetCharactersForUser(string userId);
        public void AddCharacterToUser(Character newCharacter);
        public void UpdateCharacter(Character updateThisCharacter);
        public Task DeleteCharacterAsync(string userId, int characterId);
    }

    public class CharacterRepository : ICharacterRepository
    {
        private static ApplicationDbContext _context;

        public CharacterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCharacterToUser(Character newCharacter)
        {
            try
            {
                _context.Characters.Add(newCharacter);
                _context.SaveChanges();
            }
            catch
            {

            }
        }


        // v1.0
        //public async Task DeleteCharacterAsync(string userId, int characterId)
        //{
        //    try
        //    {
        //        var characterToDelete = await _context.Characters
        //                                              .Where(c => c.User.Id == userId && c.Id == characterId)
        //                                              .SingleOrDefaultAsync();

        //        if (characterToDelete == null)
        //        {
        //            throw new Exception($"Character with ID {characterId} not found.");
        //        }

        //        _context.Characters.Remove(characterToDelete);
        //        await _context.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Unable to delete character.", ex);
        //    }
        //}

        // v2.0
        public async Task DeleteCharacterAsync(string userId, int characterId)
        {
            try
            {
                // ExecuteDeleteAsync directly executes delete operation without loading the entity into memory
                var characterToDelete = await _context.Characters
                                                      .Where(c => c.User.Id == userId && c.Id == characterId)
                                                      .ExecuteDeleteAsync();

                /* ExecuteDeleteAsync returns number of rows affected, check to make sure
                   how many rows were affected, if no rows effected, throw an exception */
                if (characterToDelete == 0)
                {
                    throw new Exception($"Character with ID {characterId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to delete character.", ex);
            }
        }

        public async Task<List<CharacterViewModel>> GetCharactersForUser(string userId)
        {
            try
            {
                var characters2 = _context.Characters
                    .Where(u => u.User.Id == userId);

                if (characters2.Count() == 0)
                {
                    throw new Exception("No characters found!");
                }

                List<CharacterViewModel> characters = await _context.Characters
                    .Where(u => u.User.Id == userId)
                    .Select(c => new CharacterViewModel()
                    {
                        Name = c.Name,
                        Age = c.Age,
                        Gender = c.Gender,
                        Level = c.Level,
                        HealthPoints = c.HealthPoints,
                        Strength = c.Strength,
                        Dexterity = c.Dexterity,
                        Intelligence = c.Intelligence,
                        Wisdom = c.Wisdom,
                        Constitution = c.Constitution,
                        Charisma = c.Charisma,
                        Backstory = c.Backstory,
                        ProfessionName = c.Profession.ProfessionName,
                        SpeciesName = c.Species.SpeciesName
                    })
                    .ToListAsync();

                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get characters");
            }
        }

        public void UpdateCharacter(Character updateThisCharacter)
        {
            throw new NotImplementedException();
        }
    }
}
