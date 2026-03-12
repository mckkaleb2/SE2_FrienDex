using FrienDex.Services;
using FrienDex.Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FrienDex.Data.ViewModels
{
    public class ReadAllPeopleVM
    {
        private readonly PersonRepo _repo;

        /// <summary>
        /// A collection of Person objects.
        /// ObservableCollections is a dynamic collection 
        /// because it listens for changes which can be 
        /// used for methods such as the ReadAll.
        /// </summary>
        public ObservableCollection<Person> People { get; set; } = new();

        /// <summary>
        /// Defines the repo for use in the ViewModel.
        /// </summary>
        /// <param name="repo">The Person Repository</param>
        public ReadAllPeopleVM(PersonRepo repo)
        {
            _repo = repo;
        }

        public async Task LoadPeople()
        {
            // Removes all Person objects from the List.
            // Prevents duplicate entries.
            People.Clear();

            // Creates a new List of Person objects which will be used to fill the main List.
            var people = await _repo.ReadAllAsync();

            // Adds a Person object to the main List of People one at a time.
            // This prompts an automatic refresh due to the ObservableCollection variable.
            foreach (var person in people)
            {
                People.Add(person);
            }
        }
    }
}
