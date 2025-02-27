﻿using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Dtos;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}