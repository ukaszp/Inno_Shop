﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Entities
{
    public class Role
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}