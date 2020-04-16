using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Qards.Pages
{
    public class QardBoardModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ShortTermMemory _memory;

        public QardBoardModel(ILogger<IndexModel> logger, IMemoryCache cache)
        {
            _logger = logger;
            _memory = new ShortTermMemory(cache);
        }
        [BindProperty]
        public string game_name { get; set; }
        [BindProperty]
        public string player_name { get; set; }

        QardsGame game = null;
        public Deck player { get; set; } = null;

        bool load(string game_name, string player_name)
        {
            this.game_name = game_name;
            this.player_name = player_name;
            game = (QardsGame)_memory.Get(game_name);
            if (game == null)
                return false;
            player = game.GetPlayer(player_name);
            return true;
        }
        public IActionResult OnGet(string game_name, string player_name)
        {
            var flag = load(game_name, player_name);
            if (!flag)
                return RedirectToPage("Error", new { Details = "can't fine game " + game_name });
            return Page();
        }

        public IActionResult OnPostDrawCard(string game_name, string player_name)
        {
            var flag = load(game_name, player_name);
            if (!flag)
                return RedirectToPage("Error", new { Details = "can't fine game " + game_name });
            game.DrawCard(player_name);
            return Page();
        }
    }
}