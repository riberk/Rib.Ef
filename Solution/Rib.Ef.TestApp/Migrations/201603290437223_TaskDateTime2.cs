namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class TaskDateTime2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationTasks", "Created", c => c.DateTime(nullable: false, precision: 2, storeType: "datetime2",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "GETUTCDATE()")
                    },
                }));
            DropColumn("dbo.ApplicationTasks", "Craeted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationTasks", "Craeted", c => c.DateTime(nullable: false));
            DropColumn("dbo.ApplicationTasks", "Created",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "SqlDefaultValue", "GETUTCDATE()" },
                });
        }
    }
}
