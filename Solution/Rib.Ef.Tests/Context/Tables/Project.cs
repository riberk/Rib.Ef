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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Description("Заголовок проекта")]
        public string Name { get; set; }

        [Description("Дата создания записи")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DateTimePrecision(0)]
        [SqlDefaultValue("GETUTCDATE()")]
        public DateTime Created { get; set; }

        [Description("Дата изменения записи")]
        [DateTimePrecision(2)]
        public DateTime LastModified { get; set; }

        [Description("Выделенная сумма")]
        [DecimalPrecision(10, 4)]
        public decimal? Money { get; set; }
    }
}