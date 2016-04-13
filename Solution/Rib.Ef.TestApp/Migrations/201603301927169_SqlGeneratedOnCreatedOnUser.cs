namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SqlGeneratedOnCreatedOnUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Created", c => c.DateTime(nullable: false, precision: 2, storeType: "datetime2",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Дата создания")
                    },
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "GETUTCDATE()")
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: "Дата создания", newValue: null)
                    },
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: "GETUTCDATE()", newValue: null)
                    },
                }));
        }
    }
}
