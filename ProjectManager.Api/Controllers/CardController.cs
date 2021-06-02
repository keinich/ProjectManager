using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Models;
using ProjectManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Api.Controllers {

  [ApiController]
  [Route("api/cards")]
  public class CardController : ControllerBase {
    private readonly AppDbContext dbContext;

    public CardController(AppDbContext dbContext) {
      this.dbContext = dbContext;
    }

    // /api/cards
    [HttpGet]
    public IEnumerable<Card> All() => dbContext.Cards;

    [HttpGet("test")]
    public string TestAuth() => "test";

    // /api/cards/{id}
    [HttpGet("{id}")]
    public Card Get(int id) => dbContext.Cards.FirstOrDefault(x => x.Id.Equals(id));

    // /api/cards
    [HttpPost]
    public async Task<Card> Create([FromBody] Card card) {
      dbContext.Cards.Add(card);
      await dbContext.SaveChangesAsync();
      Console.WriteLine("Card created");
      return card;
    }

    // /api/cards
    [HttpPut]
    public async Task<Card> Update([FromBody] Card card) {
      if (card.Id == 0) {
        return null;
      }
      dbContext.Add(card);
      await dbContext.SaveChangesAsync();
      return card;
    }

    // /api/cards/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
      Card cardToDelete = dbContext.Cards.FirstOrDefault(x => x.Id.Equals(id));
      dbContext.Cards.Remove(cardToDelete);
      await dbContext.SaveChangesAsync();
      return Ok();
    }
  }
}
