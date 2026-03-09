using FrienDex.Data.Entities;
using FrienDex.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace FrienDex.Data.ViewModels
{
    public class CreatePersonVM : INotifyPropertyChanged
    {
        private readonly IPersonRepo _repo;

        // 1.) Create Transcient Object - what the entry field binds to
        public Person newPerson { get; set; } = new Person();

        // 2.) Create Command - what the button binds to
        public ICommand SaveCommand { get; }

        public CreatePersonVM(IPersonRepo repo)
        {
            _repo = repo;

            SaveCommand = new Command(async () => await OnSave());
        }

        private async Task OnSave()
        {
            // 3.) Save to Repo
            await _repo.CreateAsync(newPerson);

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"\n\n\n\n\t\tNew Person Created: {newPerson.FirstName} {newPerson.LastName}");
            System.Diagnostics.Debug.WriteLine($"\n\nNew Person: {newPerson.ToString()}\n\n\n");
#endif

            // 4.) Reset Transcient Object
            newPerson = new Person();
            OnPropertyChanged(nameof(newPerson));

            await Shell.Current.DisplayAlertAsync("Success", "New entry successfully created.", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
