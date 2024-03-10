using System;
using System.Collections.Generic;

namespace WebApiForGptBlazor.Models;
    
public partial class Message
{
    public int Id { get; set; }

    public int? Fkchatid { get; set; }

    public string? Body { get; set; }

    public bool? Isgptauthor { get; set; }
    public Chat Chat { get; set; }  

}
