﻿namespace Rib.Ef
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
                AnnotationValues descriptionValue;
                if (columnModel.Annotations.TryGetValue(DescriptionAnnotationConvention.AnnotationName, out descriptionValue))
                {
                    AddDescription((string)descriptionValue.NewValue, createTableOperation.Name, columnModel.Name);
                }
            }
        }

        /// <summary>
        /// Override this method to generate SQL when the definition of a table or its attributes are changed.
        ///             The default implementation of this method does nothing.
        /// </summary>
        /// <param name="alterTableOperation">The operation describing changes to the table. </param>
        protected override void Generate(AlterTableOperation alterTableOperation)
        {
            base.Generate(alterTableOperation);

            AnnotationValues description;
            if (alterTableOperation.Annotations.TryGetValue(TableDescriptionAnnotationConvention.AnnotationName, out description))
            {
                AddDescription((string)description.NewValue, alterTableOperation.Name, null);
            }
        }


        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetColumnsAnnotations(alterColumnOperation.Column);

            base.Generate(alterColumnOperation);

            AnnotationValues descriptionValue;
            if (alterColumnOperation.Column.Annotations.TryGetValue(DescriptionAnnotationConvention.AnnotationName, out descriptionValue))
            {
                AddDescription((string)descriptionValue.NewValue, alterColumnOperation.Table, alterColumnOperation.Column.Name);
            }
        }


        private void AddDescription(string descriptionValue, string table, string column)
        {
            using (var writer = Writer())
            {
                string scheme;
                var tableParts = table.Split(new []{ "." }, StringSplitOptions.None);
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