namespace SCMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Dni = c.Int(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        EnrollmentDate = c.DateTime(),
                        IsTeacher = c.Boolean(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserCourse",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Course_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Course_ID })
                .ForeignKey("dbo.User", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.Course_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Course_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCourse", "Course_ID", "dbo.Course");
            DropForeignKey("dbo.UserCourse", "User_ID", "dbo.User");
            DropIndex("dbo.UserCourse", new[] { "Course_ID" });
            DropIndex("dbo.UserCourse", new[] { "User_ID" });
            DropTable("dbo.UserCourse");
            DropTable("dbo.User");
            DropTable("dbo.Course");
        }
    }
}
