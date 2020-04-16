using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Qards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ShortTermMemory _memory;

        public IndexModel(ILogger<IndexModel> logger, IMemoryCache cache)
        {
            _logger = logger;
            _memory = new ShortTermMemory(cache);
        }
        [BindProperty] 
        public string game_name { get; set; }
        [BindProperty]
        public string player_name { get; set; }
        [BindProperty]
        public IFormFile deck_name { get; set; }
        public IActionResult OnPost()
        {
            QardsGame game = null;
            if (deck_name != null)
            {
                using (var ms = new MemoryStream())
                {
                    deck_name.CopyTo(ms);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var text = sr.ReadToEnd();
                    game = new QardsGame(text);
                    _memory.Set(game_name, game);
                }
            }
            game = (QardsGame)_memory.Get(game_name);
            if (game == null)
                return RedirectToPage("Error", new { Details = "game name " + game_name + " not fount "});
            return RedirectToPage("QardBoard", new { game_name = game_name, player_name = player_name });
        }
    }
}
