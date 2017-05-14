namespace SCMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Directory", "Course_ID", c => c.Int());
            CreateIndex("dbo.Directory", "Course_ID");
            AddForeignKey("dbo.Directory", "Course_ID", "dbo.Course", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Directory", "Course_ID", "dbo.Course");
            DropIndex("dbo.Directory", new[] { "Course_ID" });
            DropColumn("dbo.Directory", "Course_ID");
        }
    }
}
