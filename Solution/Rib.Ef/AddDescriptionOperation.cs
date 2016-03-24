namespace Rib.Ef
{
    using System;
    using System.Data.Entity.Migrations.Model;
    using JetBrains.Annotations;

    public class AddDescriptionOperation : MigrationOperation
    {
        public AddDescriptionOperation([NotNull] string table, string column, [NotNull] string description)
            : base(null)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (description == null) throw new ArgumentNullException(nameof(description));
            Table = table;
            Column = column;
            Description = description;
        }

        [NotNull]
        public string Table { get; }

        public string Column { get; }

        [NotNull]
        public string Description { get; }

        public override bool IsDestructiveChange => false;
    }
}