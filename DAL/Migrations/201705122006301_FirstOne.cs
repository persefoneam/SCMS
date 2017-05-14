namespace SCMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Directory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreationDate = c.DateTime(),
                        CreatedBy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.CreatedBy_ID)
                .Index(t => t.CreatedBy_ID);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Description = c.String(),
                        UploadDate = c.DateTime(),
                        Directory_ID = c.Int(),
                        UploadBy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Directory", t => t.Directory_ID)
                .ForeignKey("dbo.User", t => t.UploadBy_ID)
                .Index(t => t.Directory_ID)
                .Index(t => t.UploadBy_ID);
            
            CreateTable(
                "dbo.EvaluatedActivity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        UploadDate = c.DateTime(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Course_ID = c.Int(),
                        CreatedBy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Course", t => t.Course_ID)
                .ForeignKey("dbo.User", t => t.CreatedBy_ID)
                .Index(t => t.Course_ID)
                .Index(t => t.CreatedBy_ID);
            
            CreateTable(
                "dbo.UserEvaluationActivity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        Score = c.Int(nullable: false),
                        EvaluatedUSer_ID = c.Int(),
                        File_ID = c.Int(),
                        EvaluatedActivity_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.EvaluatedUSer_ID)
                .ForeignKey("dbo.File", t => t.File_ID)
                .ForeignKey("dbo.EvaluatedActivity", t => t.EvaluatedActivity_ID)
                .Index(t => t.EvaluatedUSer_ID)
                .Index(t => t.File_ID)
                .Index(t => t.EvaluatedActivity_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserEvaluationActivity", "EvaluatedActivity_ID", "dbo.EvaluatedActivity");
            DropForeignKey("dbo.UserEvaluationActivity", "File_ID", "dbo.File");
            DropForeignKey("dbo.UserEvaluationActivity", "EvaluatedUSer_ID", "dbo.User");
            DropForeignKey("dbo.EvaluatedActivity", "CreatedBy_ID", "dbo.User");
            DropForeignKey("dbo.EvaluatedActivity", "Course_ID", "dbo.Course");
            DropForeignKey("dbo.File", "UploadBy_ID", "dbo.User");
            DropForeignKey("dbo.File", "Directory_ID", "dbo.Directory");
            DropForeignKey("dbo.Directory", "CreatedBy_ID", "dbo.User");
            DropIndex("dbo.UserEvaluationActivity", new[] { "EvaluatedActivity_ID" });
            DropIndex("dbo.UserEvaluationActivity", new[] { "File_ID" });
            DropIndex("dbo.UserEvaluationActivity", new[] { "EvaluatedUSer_ID" });
            DropIndex("dbo.EvaluatedActivity", new[] { "CreatedBy_ID" });
            DropIndex("dbo.EvaluatedActivity", new[] { "Course_ID" });
            DropIndex("dbo.File", new[] { "UploadBy_ID" });
            DropIndex("dbo.File", new[] { "Directory_ID" });
            DropIndex("dbo.Directory", new[] { "CreatedBy_ID" });
            DropTable("dbo.UserEvaluationActivity");
            DropTable("dbo.EvaluatedActivity");
            DropTable("dbo.File");
            DropTable("dbo.Directory");
        }
    }
}
