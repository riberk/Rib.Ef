namespace Rib.Ef
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.Entity.SqlServer;
    using System.IO;
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

            AnnotationAction<string>(
                addColumnOperation.Column.Annotations,
                DescriptionAnnotationConvention.AnnotationName,
                s => AddDescription(s, addColumnOperation.Table, addColumnOperation.Column.Name));
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
                action((T) value.NewValue);
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
                writer.WriteLine();
                writer.Write(
                    $"IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('{table}') AND [name] = N'Description' AND [minor_id] = ");
                writer.Write(
                    string.IsNullOrWhiteSpace(column)
                        ? "0"
                        : $"(SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = '{column}' AND [object_id] = OBJECT_ID('{table}'))");
                writer.Write(")");
                writer.WriteLine();
                writer.WriteLine("EXEC sys.sp_addextendedproperty");
                WriteExtendedProperty(writer, "Description", descriptionValue, scheme, table, column);
                writer.WriteLine("ELSE");
                writer.WriteLine("EXEC sys.sp_updateextendedproperty");
                WriteExtendedProperty(writer, "Description", descriptionValue, scheme, table, column);
                Statement(writer);
            }
        }

        private void WriteExtendedProperty([NotNull] TextWriter writer, string name, string value, string scheme, string table, string column)
        {
            writer.WriteLine($"@name = N'{name}',");
            writer.WriteLine($"@value = N'{value}',");
            writer.WriteLine($"@level0type = N'SCHEMA', @level0name = '{scheme}',");
            writer.WriteLine($"@level1type = N'TABLE', @level1name = '{table}'");
            if (!string.IsNullOrWhiteSpace(column))
            {
                writer.WriteLine($",@level2type = N'Column', @level2name = '{column}'");
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