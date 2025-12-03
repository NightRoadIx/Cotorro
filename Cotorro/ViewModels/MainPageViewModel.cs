using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.Controls;
using Cotorro.Services;

namespace Cotorro.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly ITtsService _ttsService;

        private string _textToRead;
        private Locale _selectedLocale;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Locale> Locales { get; } = new();

        public string TextToRead
        {
            get => _textToRead;
            set
            {
                if (_textToRead != value)
                {
                    _textToRead = value;
                    OnPropertyChanged();
                    ((Command)SpeakCommand).ChangeCanExecute();
                }
            }
        }

        public Locale SelectedLocale
        {
            get => _selectedLocale;
            set
            {
                if (_selectedLocale != value)
                {
                    _selectedLocale = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    ((Command)SpeakCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand SpeakCommand { get; }

        public MainPageViewModel(ITtsService ttsService)
        {
            _ttsService = ttsService;
            SpeakCommand = new Command(
                async () => await SpeakAsync(),
                () => !IsBusy && !string.IsNullOrWhiteSpace(TextToRead)
            );
        }

        public async Task InitializeAsync()
        {
            if (Locales.Count > 0)
                return;

            IsBusy = true;

            try
            {
                var locales = await _ttsService.GetLocalesAsync();
                Locales.Clear();

                foreach (var locale in locales)
                    Locales.Add(locale);

                // Por defecto, el idioma del sistema
                if (Locales.Count > 0)
                    SelectedLocale = Locales[0];
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SpeakAsync()
        {
            if (string.IsNullOrWhiteSpace(TextToRead))
                return;

            IsBusy = true;
            try
            {
                await _ttsService.SpeakAsync(TextToRead, SelectedLocale);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
