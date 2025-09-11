using System;

namespace SMS.EPA.Core.Extensions.XML;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class XmlNameAttribute(string name) : System.Attribute
{
    public string Name { get; } = name;
}