﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Villa_ResfulAPI.Models.DTO
{
    public class VillaNumberCreateDto
    {
        [Required]
		[Range(100, 1000)]
		public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
