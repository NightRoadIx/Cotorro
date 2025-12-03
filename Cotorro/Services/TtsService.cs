using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

namespace Cotorro.Services
{
    public class TtsService : ITtsService
    {
        private CancellationTokenSource _cts;

        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            return locales;
        }

        public async Task SpeakAsync(string text, Locale locale = null)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var options = new SpeechOptions
            {
                Volume = 1.0f,
                Pitch = 1.0f,
                Locale = locale
            };

            await TextToSpeech.Default.SpeakAsync(text, options, _cts.Token);
        }
    }
}
