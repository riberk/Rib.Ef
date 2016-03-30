namespace Rib.Ef.Tests.Context.Tables
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rib.Ef.Metadata;
    using Rib.Ef.Tests.Context.Interfaces;

    public class User : IHasUpdateDate
    {
        public int Id { get; set; }

        public string Login { get; set; }

        [DateTimePrecision(2)]
        [Description("Дата создания")]
        [SqlDefaultValue("GETUTCDATE()")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(nameof(Created), TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [DateTimePrecision(2)]
        [Description("Дата последнего изменения")]
        [SqlDefaultValue("GETUTCDATE()")]
        [Column(nameof(Modified), TypeName = "datetime2")]
        public DateTime Modified { get; set; }
    }
}