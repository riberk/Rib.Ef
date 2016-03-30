namespace Rib.Ef.Tests.Context.Interfaces
{
    using System;

    public interface IHasUpdateDate
    {
        DateTime Modified { get; set; } 
    }
}