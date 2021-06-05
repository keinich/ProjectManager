using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ProjectManager.Api.Models {

  public class Card {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
  }

}
