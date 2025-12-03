using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

namespace Cotorro.Services
{
    public interface ITtsService
    {
        Task<IEnumerable<Locale>> GetLocalesAsync();
        Task SpeakAsync(string text, Locale locale = null);
    }
}