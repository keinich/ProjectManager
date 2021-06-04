using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Models;
using ProjectManager.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManager.Api.Controllers {

  [ApiController]
  [Route("api/cards")]
  public class CardController : ControllerBase {
    private readonly AppDbContext dbContext;

    public CardController(AppDbContext dbContext) {
      this.dbContext = dbContext;
    }

    //GET /api/cards
    [Authorize]
    [HttpGet]
    public IEnumerable<Card> All() {
      Debug.WriteLine($"UserId: {this.User.FindFirst(ClaimTypes.NameIdentifier).Value}");
      return dbContext.Cards;
    }

    //GET /api/cards/{id}
    [Authorize]
    [HttpGet("{userId}")]
    public ActionResult<IEnumerable<Card>> Get(string userId) {
      if (userId != this.User.FindFirst(ClaimTypes.NameIdentifier).Value) {
        return Unauthorized();
      }
      IEnumerable<Card> result = dbContext.Cards.Where(x => x.UserId == userId);
      return Ok(result);
    }

    //POST /api/cards
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Card>> Create([FromBody] Card card) {
      if (card.UserId != this.User.FindFirst(ClaimTypes.NameIdentifier).Value) {
        return Unauthorized();
      }
      dbContext.Cards.Add(card);
      await dbContext.SaveChangesAsync();
      Console.WriteLine("Card created");
      return Ok(card);
    }

    //PUT /api/cards
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
