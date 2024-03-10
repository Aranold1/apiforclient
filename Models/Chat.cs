using System;
using System.Collections.Generic;

namespace WebApiForGptBlazor.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int? Fkuserid { get; set; }

    public string? Theme { get; set; }
    public virtual User? Fkuser { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
