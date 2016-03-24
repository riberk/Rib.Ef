namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Идентификатор записи")
                                },
                            }),
                        Name = c.String(nullable: false),
                        Created = c.DateTime(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Дата создания записи")
                                },
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "GETUTCDATE()")
                                },
                            }),
                        Money = c.Decimal(precision: 10, scale: 4),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "Description", "Проекты" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Projects",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "Description", "Проекты" },
                },
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Created",
                        new Dictionary<string, object>
                        {
                            { "Description", "Дата создания записи" },
                            { "SqlDefaultValue", "GETUTCDATE()" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "Description", "Идентификатор записи" },
                        }
                    },
                });
        }
    }
}
