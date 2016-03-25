namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDescriptions : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Идентификатор записи", newValue: null)
                    },
                }));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Наименование проекта", newValue: "Заголовок проекта")
                    },
                }));
            AlterColumn("dbo.Projects", "Money", c => c.Decimal(precision: 10, scale: 4,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Выделенная сумма")
                    },
                }));
            AddPrimaryKey("dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.Projects", "Money", c => c.Decimal(precision: 10, scale: 4,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Выделенная сумма", newValue: null)
                    },
                }));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Заголовок проекта", newValue: "Наименование проекта")
                    },
                }));
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Идентификатор записи")
                    },
                }));
            AddPrimaryKey("dbo.Projects", "Id");
        }
    }
}
