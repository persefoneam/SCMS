namespace SCMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastchanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Email", c => c.String());
        }
    }
}
