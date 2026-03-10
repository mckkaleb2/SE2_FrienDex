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

        // 1.) Create Transient Object - what the entry field binds to
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

            // 4.) Reset Transient Object
            newPerson = new Person();
            OnPropertyChanged(nameof(newPerson));

            string doesItWork = "";

            await doesItWork = _repo.ReadAsync()

            await Shell.Current.DisplayAlertAsync("Success", , "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
