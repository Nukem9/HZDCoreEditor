﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "Commands using the 'Examples' static field turn into a mess otherwise", Scope = "namespaceanddescendants", Target = "~N:HZDCoreTools")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Command classes are defined at the top for readability", Scope = "namespaceanddescendants", Target = "~N:HZDCoreTools")]
