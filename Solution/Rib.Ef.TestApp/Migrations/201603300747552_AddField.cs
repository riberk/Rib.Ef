namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Description", c => c.String(
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Description",
                        new AnnotationValues(oldValue: null, newValue: "Описание")
                    },
                }));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Description",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "Description", "Описание" },
                });
        }
    }
}
