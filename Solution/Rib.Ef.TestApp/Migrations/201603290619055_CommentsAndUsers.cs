namespace Rib.Ef.Tests.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsAndUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        UserId = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 2, storeType: "datetime2",
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
                            }),
                        Modified = c.DateTime(nullable: false, precision: 2, storeType: "datetime2",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Дата последнего изменения")
                                },
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "GETUTCDATE()")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationTasks", t => t.TaskId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Modified = c.DateTime(nullable: false, precision: 2, storeType: "datetime2",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Description",
                                    new AnnotationValues(oldValue: null, newValue: "Дата последнего изменения")
                                },
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "GETUTCDATE()")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "TaskId", "dbo.ApplicationTasks");
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Users",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Modified",
                        new Dictionary<string, object>
                        {
                            { "Description", "Дата последнего изменения" },
                            { "SqlDefaultValue", "GETUTCDATE()" },
                        }
                    },
                });
            DropTable("dbo.Comments",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Created",
                        new Dictionary<string, object>
                        {
                            { "Description", "Дата создания" },
                            { "SqlDefaultValue", "GETUTCDATE()" },
                        }
                    },
                    {
                        "Modified",
                        new Dictionary<string, object>
                        {
                            { "Description", "Дата последнего изменения" },
                            { "SqlDefaultValue", "GETUTCDATE()" },
                        }
                    },
                });
        }
    }
}
