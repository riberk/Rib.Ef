namespace Rib.Ef.Tests.Context.Tables
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rib.Ef.Metadata;

    public class ApplicationTask
    {
        [Description("Идентификатор записи")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Description("Заголовок задачи")]
        public string Title { get; set; }

        [Description("Идентификатор проекта")]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        [DateTimePrecision(2)]
        [SqlDefaultValue("GETUTCDATE()")]
        [Column(nameof(Created), TypeName = "datetime2")]
        public DateTime Created { get; set; }
    }
}