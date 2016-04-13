namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastModifiedWithAnotAndAddAnotToName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "LastModified", c => c.DateTime(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Дата изменения записи")
                    },
                }));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Наименование проекта")
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Наименование проекта", newValue: null)
                    },
                }));
            DropColumn("dbo.Projects", "LastModified",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "Description", "Дата изменения записи" },
                });
        }
    }
}
