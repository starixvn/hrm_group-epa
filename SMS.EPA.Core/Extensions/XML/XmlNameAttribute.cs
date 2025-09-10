namespace SMS.EPA.Core.Extensions;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class XmlNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}