﻿using System;
using System.Collections.Generic;

namespace WebApiForGptBlazor.Models;

public partial class User
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int Id { get; set; }
}
