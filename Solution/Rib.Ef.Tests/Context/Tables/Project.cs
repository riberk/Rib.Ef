namespace Rib.Ef.Tests.Context.Tables
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rib.Ef.Metadata;

    [Table("Projects")]
    [Description("Проекты")]
    public class Project
    {
        [Description("Идентификатор записи")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Description("Наименование проекта")]
        public string Name { get; set; }

        [Description("Дата создания записи")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DateTimePrecision(0)]
        [SqlDefaultValue("GETUTCDATE()")]
        public DateTime Created { get; set; }

        [Description("Дата изменения записи")]
        [DateTimePrecision(2)]
        public DateTime LastModified { get; set; }

        [DecimalPrecision(10, 4)]
        public decimal? Money { get; set; }
    }
}