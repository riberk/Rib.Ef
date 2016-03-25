namespace Rib.Ef
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using JetBrains.Annotations;
    using Rib.Ef.Conventions;

    public class RibEfSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        public RibEfSqlServerMigrationSqlGenerator()
        {
            SchemeResolver = () => "dbo";
        }

        [NotNull]
        public Func<string> SchemeResolver { get; set; }

        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetColumnsAnnotations(addColumnOperation.Column);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetColumnsAnnotations(createTableOperation.Columns, createTableOperation.Name);

            base.Generate(createTableOperation);

            object desc;
            string description;
            if (createTableOperation.Annotations.TryGetValue(TableDescriptionAnnotationConvention.AnnotationName, out desc)
                && (description = desc as string) != null)
            {
                AddDescription(description, createTableOperation.Name, null);
            }

            foreach (var columnModel in createTableOperation.Columns)
            {
                AddDescriptionToColumnFromAnnoatation(DescriptionAnnotationConvention.AnnotationName, createTableOperation.Name, columnModel);
            }
        }

        /// <summary>
        ///     Override this method to generate SQL when the definition of a table or its attributes are changed.
        ///     The default implementation of this method does nothing.
        /// </summary>
        /// <param name="alterTableOperation">The operation describing changes to the table. </param>
        protected override void Generate(AlterTableOperation alterTableOperation)
        {
            base.Generate(alterTableOperation);

            AnnotationAction<string>(
                alterTableOperation.Annotations,
                TableDescriptionAnnotationConvention.AnnotationName,
                s => AddDescription(s, alterTableOperation.Name, null));
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetColumnsAnnotations(alterColumnOperation.Column);

            base.Generate(alterColumnOperation);

            AnnotationAction<string>(
                alterColumnOperation.Column.Annotations,
                DescriptionAnnotationConvention.AnnotationName,
                s => AddDescription(s, alterColumnOperation.Table, alterColumnOperation.Column.Name));
        }

        private void AddDescriptionToColumnFromAnnoatation([NotNull] string annotationName, [NotNull] string table, [NotNull] ColumnModel column)
        {
            AnnotationAction<string>(column.Annotations, annotationName, s => AddDescription(s, table, column.Name));
        }

        private static void AnnotationAction<T>(
            [NotNull] IDictionary<string, AnnotationValues> dict,
            [NotNull] string name,
            [NotNull] Action<T> action)
        {
            AnnotationValues value;
            if (dict.TryGetValue(name, out value))
            {
                action((T)value.NewValue);
            }
        }

        private void AddDescription(string descriptionValue, string table, string column)
        {
            using (var writer = Writer())
            {
                string scheme;
                var tableParts = table.Split(new[] {"."}, StringSplitOptions.None);
                if (tableParts.Length == 2)
                {
                    table = tableParts[1];
                    scheme = tableParts[0];
                }
                else if (tableParts.Length == 1)
                {
                    table = tableParts[0];
                    scheme = SchemeResolver();
                }
                else
                {
                    throw new InvalidOperationException();
                }
                /*
                
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('Table_Name') AND [name] = N'MS_Description' AND [minor_id] = 0)
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This table is responsible for holding information.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Table_Name';

IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('Table_Name') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Column_Name' AND [object_id] = OBJECT_ID('Table_Name')))
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This column is responsible for holding information for table Table_Name.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Table_Name', @level2type = N'COLUMN', @level2name = N'Column_Name';
                */
                writer.WriteLine("EXEC sys.sp_addextendedproperty");
                writer.WriteLine("@name = N'Description',");
                writer.WriteLine($"@value = N'{descriptionValue}',");
                writer.WriteLine($"@level0type = N'SCHEMA', @level0name = '{scheme}',");
                writer.WriteLine($"@level1type = N'TABLE', @level1name = '{table}'");
                if (!string.IsNullOrWhiteSpace(column))
                {
                    writer.WriteLine($",@level2type = N'Column', @level2name = '{column}'");
                }
                Statement(writer);
            }
        }

        private void SetColumnsAnnotations(IEnumerable<ColumnModel> columns, string table)
        {
            foreach (var columnModel in columns)
            {
                SetColumnsAnnotations(columnModel);
            }
        }

        private void SetColumnsAnnotations(ColumnModel column)
        {
            AnnotationValues defaultValueSql;
            AnnotationValues defaultValue;

            if (column.Annotations.TryGetValue(SqlDefaultValueAnnotationConvention.AnnotationName, out defaultValueSql))
            {
                column.DefaultValueSql = (string) defaultValueSql.NewValue;
            }
            if (column.Annotations.TryGetValue(DefaultValueAnnotationConvention.AnnotationName, out defaultValue))
            {
                column.DefaultValue = defaultValue.NewValue;
            }
        }
    }
}