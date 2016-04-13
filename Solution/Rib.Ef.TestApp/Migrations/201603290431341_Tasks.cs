namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Tasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationTasks",
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
                        Title = c.String(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Заголовок задачи")
                                },
                            }),
                        ProjectId = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Идентификатор проекта")
                                },
                            }),
                        Craeted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationTasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ApplicationTasks", new[] { "ProjectId" });
            DropTable("dbo.ApplicationTasks",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "Description", "Идентификатор записи" },
                        }
                    },
                    {
                        "ProjectId",
                        new Dictionary<string, object>
                        {
                            { "Description", "Идентификатор проекта" },
                        }
                    },
                    {
                        "Title",
                        new Dictionary<string, object>
                        {
                            { "Description", "Заголовок задачи" },
                        }
                    },
                });
        }
    }
}
