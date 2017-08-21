﻿using System;

namespace AstralKeks.PowerShell.Common.Attributes
{
    public class DynamicParameterAttribute : Attribute
    {
        public int Position { get; set; } = -1;
        public bool Mandatory { get; set; } = false;
        public bool ValueFromPipeline { get; set; } = false;
        public string ParameterSetName { get; set; }
    }
}