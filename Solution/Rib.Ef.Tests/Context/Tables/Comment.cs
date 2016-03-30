namespace Rib.Ef.Tests.Context.Tables
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rib.Ef.Metadata;

    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int TaskId { get; set; }

        public ApplicationTask Task { get; set; }

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