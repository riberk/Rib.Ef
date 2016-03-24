namespace Rib.Ef
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using Rib.Ef.Conventions;

    public class RibEfSqlServerMigrationSqlGeneratorBase : SqlServerMigrationSqlGenerator
    {
    }

    public class RibEfSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetDefaultValues(addColumnOperation.Column);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetDefaultValues(createTableOperation.Columns);

            base.Generate(createTableOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetDefaultValues(alterColumnOperation.Column);

            base.Generate(alterColumnOperation);
        }

        private static void SetDefaultValues(IEnumerable<ColumnModel> columns)
        {
            foreach (var columnModel in columns)
            {
                SetDefaultValues(columnModel);
            }
        }

        private static void SetDefaultValues(ColumnModel column)
        {
            AnnotationValues defaultValueSql;
            AnnotationValues defaultValue;
            AnnotationValues descriptionValue;
            if (column.Annotations.TryGetValue(SqlDefaultValueAnnotationConvention.AnnotationName, out defaultValueSql))
            {
                column.DefaultValueSql = (string) defaultValueSql.NewValue;
            }
            if (column.Annotations.TryGetValue(DefaultValueAnnotationConvention.AnnotationName, out defaultValue))
            {
                column.DefaultValue = defaultValue.NewValue;
            }
            if (column.Annotations.TryGetValue(DescriptionAnnotationConvention.AnnotationName, out descriptionValue))
            {
                using (var writer = Writer())
                {
                    //                    EXEC sp_addextendedproperty
                    //@name = N'Description', @value = 'Hey, here is my description!',
                    //@level0type = N'Schema', @level0name = yourschema,
                    //@level1type = N'Table',  @level1name = YourTable,
                    //@level2type = N'Column', @level2name = yourColumn;
                    //                    GO


//                    USE AdventureWorks2012;
//                    GO
//                    EXEC sys.sp_addextendedproperty
//                    @name = N'MS_DescriptionExample', 
//@value = N'Street address information for customers, employees, and vendors.', 
//@level0type = N'SCHEMA', @level0name = 'Person',
//@level1type = N'TABLE',  @level1name = 'Address';
//                    GO
                }
            }
            
        }
    }
}